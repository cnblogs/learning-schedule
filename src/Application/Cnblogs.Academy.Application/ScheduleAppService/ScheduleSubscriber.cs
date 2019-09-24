using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Enyim.Caching;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleSubscriber : IScheduleSubscriber, ICapSubscribe
    {
        private readonly IMsgApiService _msgSvc;
        private readonly IUCenterService _uCenter;
        private readonly IScheduleRepository _repository;
        private readonly IMemcachedClient _cache;

        public ScheduleSubscriber(IMsgApiService msgSvc, IUCenterService uCenter, IScheduleRepository repository, IMemcachedClient cache)
        {
            _msgSvc = msgSvc;
            _uCenter = uCenter;
            _repository = repository;
            _cache = cache;
        }

        [CapSubscribe(EventConst.ScheduleUpdatedEvent, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleScheduleUpdatedEvent(ScheduleUpdatedEvent e)
        {
            var schedule = await _repository.FindByUUID<Schedule>(e.ScheduleUuid).FirstOrDefaultAsync();
            if (schedule == null) return;

            int page = 0, count = 10;
            do
            {
                var followingSchedules = await _repository.Schedules.Where(x => x.ParentId == schedule.Id)
                    .OrderBy(x => x.Id)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                count = followingSchedules.Count;
                page++;

                foreach (var child in followingSchedules)
                {
                    child.Update(schedule.Title,
                                 schedule.Description,
                                 schedule.IsPrivate,
                                 operatorId: schedule.UserId);
                }
                await _repository.UnitOfWork.SaveEntitiesAsync();

            } while (count >= 10);
        }

        [CapSubscribe(EventConst.NewSubscriber, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleNewSubscriberEvent(NewSubscriberEvent e)
        {
            var id = await _repository.FindByUUID<Schedule>(e.ParentScheduleUuid).Select(x => x.Id).FirstOrDefaultAsync();
            await _cache.RemoveAsync(CacheKeyStore.ScheduleFollowings(id));
        }

        [CapSubscribe(EventConst.UnsubscribeEvent, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleUnsubscribeEvent(UnsubscribeEvent e)
        {
            await _cache.RemoveAsync(CacheKeyStore.ScheduleFollowings(e.ParentId));
        }
    }
}
