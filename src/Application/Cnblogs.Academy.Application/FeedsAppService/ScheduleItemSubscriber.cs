using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.FeedService;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Enyim.Caching;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public class ScheduleItemSubscriber : IScheduleItemSubscriber, ICapSubscribe
    {
        private readonly IFeedServiceAgent _feedSvc;
        private readonly IUCenterService _uCenter;
        private readonly IMemcachedClient _cache;
        private readonly IScheduleRepository _repository;
        private readonly IScheduleService _scheduleSvc;

        public ScheduleItemSubscriber(
            IFeedServiceAgent feedSvc,
            IUCenterService uCenter,
            IMemcachedClient cache,
            IScheduleRepository repository,
            IScheduleService scheduleSvc)
        {
            _uCenter = uCenter;
            _feedSvc = feedSvc;
            _cache = cache;
            _repository = repository;
            _scheduleSvc = scheduleSvc;
        }

        [CapSubscribe(EventConst.ScheduleItemCompletedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleItemCompletedEvent(Domain.Schedules.Events.ScheduleItemCompletedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                           .Include(x => x.Schedule)
                           .Include(x => x.Html)
                           .FirstOrDefaultAsync();
            if (item == null) return;

            var subscriber = await _uCenter.GetUser(x => x.UserId, item.UserId);
            if (subscriber == null) return;

            await _feedSvc.PublishAsync(new FeedInputModel
            {
                ContentId = item.Id.ToString(),
                FeedTitle = item.GenerateDescription(),
                FeedContent = item.Schedule.Title,
                Link = $"{AppConst.DomainAddress}/schedules/u/{subscriber.Alias}/{item.ScheduleId}/detail/{item.Id}",
                UserId = item.UserId,
                AppId = AppConst.AppGuid,
                FeedType = FeedType.ScheduleItemDone,
                IsPrivate = item.Schedule.IsPrivate
            });
            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
        }

        [CapSubscribe(EventConst.ScheduleItemCreatedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleItemCreatedEvent(Cnblogs.Academy.Domain.Schedules.Events.ScheduleItemCreatedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                .Include(x => x.Schedule)
                .Include(x => x.Html)
                .FirstOrDefaultAsync();

            if (item == null) return;

            if (item.ParentId > 0)
            {
                return; //child item 不需要发布动态
            }
            var alias = await _uCenter.GetUser(x => x.UserId, item.UserId, x => x.Alias);
            if (string.IsNullOrEmpty(alias)) return;

            await _feedSvc.PublishAsync(new Feed.DTO.FeedInputModel
            {
                ContentId = item.Id.ToString(),
                FeedTitle = item.GenerateDescription(),
                Link = $"{AppConst.DomainAddress}/schedules/u/{alias}/{item.ScheduleId}/item/{item.Id}",
                UserId = item.UserId,
                IsPrivate = item.Schedule.IsPrivate,
                AppId = AppConst.AppGuid,
                FeedType = Feed.ValueObjects.FeedType.ScheduleItemNew,
            });
            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
        }

        [CapSubscribe(EventConst.ScheduleItemDeletedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleItemDeletedEvent(Domain.Schedules.Events.ScheduleItemDeletedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                            .IgnoreQueryFilters()
                            .Include(x => x.Schedule)
                            .FirstOrDefaultAsync();

            if (item == null) return;

            var model = new Feed.DTO.FeedDeletedInput
            {
                ContentId = item.Id.ToString(),
                AppId = AppConst.AppGuid,
                UserId = item.Schedule.UserId,
                FeedType = Feed.ValueObjects.FeedType.ScheduleItemNew
            };
            await _feedSvc.DeleteAsync(model);

            if (item.Completed)
            {
                model.FeedType = Feed.ValueObjects.FeedType.ScheduleItemDone;
                await _feedSvc.DeleteAsync(model);
            }

            if (_scheduleSvc.ShouldClearCache(item.DateAdded) || _scheduleSvc.ShouldClearCache(item.DateEnd))
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
        }

        [CapSubscribe(EventConst.ScheduleItemUndoEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleItemUndoEvent(ScheduleItemUndoEvent e)
        {
            await _feedSvc.DeleteAsync(new Feed.DTO.FeedDeletedInput
            {
                AppId = AppConst.AppGuid,
                FeedType = Feed.ValueObjects.FeedType.ScheduleItemDone,
                ContentId = e.ItemId.ToString(),
                UserId = e.UserId
            });
            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
        }

        [CapSubscribe(EventConst.ScheduleItemUpdatedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleItemUpdatedEvent(Domain.Schedules.Events.ScheduleItemUpdatedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid).Include(x => x.Html).FirstOrDefaultAsync();
            if (item == null) return;

            // Update feeds
            var model = new Feed.DTO.FeedUpdateModel
            {
                ContentId = item.Id.ToString(),
                FeedTitle = item.GenerateDescription(),
                FeedType = Feed.ValueObjects.FeedType.ScheduleItemNew
            };
            await _feedSvc.UpdateAsync(model);

            if (item.Completed)
            {
                model.FeedType = Feed.ValueObjects.FeedType.ScheduleItemDone;
                await _feedSvc.UpdateAsync(model);
            }

            var will = _scheduleSvc.ShouldClearCache(item.DateAdded) || _scheduleSvc.ShouldClearCache(item.DateEnd);

            if (will)
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
        }
    }
}
