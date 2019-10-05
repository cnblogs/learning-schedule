using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.FeedService;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Enyim.Caching;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public class ScheduleSubscriber : IScheduleSubscriber, ICapSubscribe
    {
        private readonly IFeedServiceAgent _feedSvc;
        private readonly IUCenterService _uCenter;
        private readonly IMemcachedClient _cache;
        private readonly IScheduleRepository _repository;
        private readonly IMsgApiService _msgSvc;
        private readonly IScheduleService _scheduleSvc;

        public ScheduleSubscriber(
            IFeedServiceAgent feedSvc,
            IUCenterService uCenter,
            IMemcachedClient cache,
            IScheduleRepository repository,
            IMsgApiService msgSvc,
            IScheduleService scheduleSvc)
        {
            _feedSvc = feedSvc;
            _uCenter = uCenter;
            _cache = cache;
            _repository = repository;
            _msgSvc = msgSvc;
            _scheduleSvc = scheduleSvc;
        }

        [CapSubscribe(EventConst.ScheduleCompletedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleCompletedEvent(ScheduleCompletedEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (schedule == null) return;

            var user = await _uCenter.GetUser(x => x.UserId, schedule.UserId);
            if (user == null) return;

            await _feedSvc.PublishAsync(new FeedInputModel
            {
                AppId = AppConst.AppGuid,
                FeedType = FeedType.ScheduleCompleted,
                ContentId = schedule.Id.ToString(),
                UserId = schedule.UserId,
                FeedTitle = schedule.Title,
                IsPrivate = schedule.IsPrivate,
                Link = $"{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}"
            });

            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
        }

        [CapSubscribe(EventConst.ScheduleCreatedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleCreatedEvent(ScheduleCreatedEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (schedule == null) return;

            if (schedule.ParentId > 0)
            {
                return; // Not publish feed if it's a subscribed schedule
            }
            var user = await _uCenter.GetUser(x => x.UserId, schedule.UserId);
            if (user == null) return;

            await _feedSvc.PublishAsync(new FeedInputModel
            {
                ContentId = schedule.Id.ToString(),
                FeedTitle = schedule.Title,
                FeedContent = schedule.Description,
                UserId = schedule.UserId,
                Link = $"{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}",
                IsPrivate = schedule.IsPrivate,
                AppId = AppConst.AppGuid,
                FeedType = FeedType.ScheduleNew
            });
            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
        }


        [CapSubscribe(EventConst.ScheduleCancelledEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleCancelledEvent(ScheduleCancelledEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (schedule == null) return;

            await _feedSvc.DeleteAsync(new FeedDeletedInput
            {
                AppId = AppConst.AppGuid,
                FeedType = FeedType.ScheduleCompleted,
                UserId = schedule.UserId,
                ContentId = schedule.Id.ToString()
            });

            if (_scheduleSvc.ShouldClearCache(schedule.DateUpdated))
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
        }

        [CapSubscribe(EventConst.ScheduleDeletedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleDeletedEvent(ScheduleDeletedEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).IgnoreQueryFilters().FirstOrDefaultAsync();
            if (schedule == null) return;

            var model = new FeedDeletedInput
            {
                ContentId = schedule.Id.ToString(),
                FeedType = FeedType.ScheduleNew,
                AppId = AppConst.AppGuid,
                UserId = schedule.UserId
            };
            await _feedSvc.DeleteAsync(model);
            if (schedule.Stage == Domain.Schedules.Stage.Completed)
            {
                model.FeedType = FeedType.ScheduleCompleted;
                await _feedSvc.DeleteAsync(model);
            }

            if (_scheduleSvc.ShouldClearCache(schedule.DateUpdated))
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
        }

        [CapSubscribe(EventConst.ScheduleUpdatedEvent, Group = FeedAppConst.MessageGroup)]
        public async Task HandleScheduleUpdatedEvent(ScheduleUpdatedEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (schedule == null) return;

            var model = new FeedUpdateModel
            {
                ContentId = schedule.Id.ToString(),
                FeedTitle = schedule.Title,
                FeedContent = schedule.Description,
                FeedType = FeedType.ScheduleNew
            };

            await _feedSvc.UpdateAsync(model);

            if (schedule.Stage == Stage.Completed)
            {
                model.FeedType = FeedType.ScheduleCompleted;
                await _feedSvc.UpdateAsync(model);
            }

            if (_scheduleSvc.ShouldClearCache(schedule.DateAdded) || _scheduleSvc.ShouldClearCache(schedule.DateEnd))
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
        }
    }
}
