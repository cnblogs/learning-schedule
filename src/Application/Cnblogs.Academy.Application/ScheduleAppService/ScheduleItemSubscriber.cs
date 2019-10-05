using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Events;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleItemSubscriber : IScheduleItemSubscriber, ICapSubscribe
    {
        private readonly IMsgApiService _msgSvc;
        private readonly IUCenterService _uCenter;
        private readonly IScheduleRepository _repository;

        public ScheduleItemSubscriber(IMsgApiService msgSvc, IUCenterService uCenter, IScheduleRepository repository)
        {
            _msgSvc = msgSvc;
            _uCenter = uCenter;
            _repository = repository;
        }

        [CapSubscribe(EventConst.ScheduleItemCreatedEvent, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleScheduleItemCreatedEvent(ScheduleItemCreatedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid, tracking: true)
               .Include(x => x.Schedule)
               .Include(x => x.Html)
               .FirstOrDefaultAsync();

            if (item == null) return;

            int page = 0, count = 10;
            do
            {
                var schedules = await _repository.Schedules.Where(x => x.ParentId == item.ScheduleId)
                    .OrderBy(x => x.Id)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                page++;
                count = schedules.Count;

                foreach (var schedule in schedules)
                {
                    var child = item.Dispatch(schedule.Id, schedule.UserId);
                    schedule.AddItem(child);
                }
                try
                {
                    await _repository.UnitOfWork.SaveEntitiesAsync();
                }
                catch (Exception ex) when (ex.InnerException is SqlException sqlerror)
                {
                    if (sqlerror.Number == 2601)
                    {
                        // Ignore existed item to implement Idempotency;
                    }
                    else throw ex;
                }
            }
            while (count >= 10);
        }

        [CapSubscribe(EventConst.ScheduleItemDeletedEvent, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleScheduleItemDeletedEvent(ScheduleItemDeletedEvent e)
        {
            var itemId = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid)
                .IgnoreQueryFilters()
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (itemId < 1) return;

            int page = 0, count = 10;
            do
            {
                var followingItems = await _repository.ScheduleItems
                    .Include(x => x.Schedule)
                    .Include(x => x.Html)
                    .Where(x => x.ParentId == itemId)
                    .OrderBy(x => x.Id)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                count = followingItems.Count;
                page++;

                foreach (var child in followingItems)
                {
                    child.Schedule.DeleteItem(child.Id);
                }

                await _repository.UnitOfWork.SaveEntitiesAsync();
            } while (count >= 10);
        }

        [CapSubscribe(EventConst.ScheduleItemUpdatedEvent, Group = ScheduleAppConst.MessageGroup)]
        public async Task HandleScheduleItemUpdatedEvent(ScheduleItemUpdatedEvent e)
        {
            var item = await _repository.FindByUUID<ScheduleItem>(e.ItemUuid).Include(x => x.Html).FirstOrDefaultAsync();
            if (item == null) return;

            // Dispatch to following items
            int page = 0, count = 10;
            do
            {
                var followingItems = await _repository.ScheduleItems.Include(x => x.Schedule)
                    .Include(x => x.Html)
                    .Where(x => x.ParentId == item.Id)
                    .OrderBy(x => x.Id)
                    .Skip(page * count)
                    .Take(count)
                    .ToListAsync();

                count = followingItems.Count;
                page++;

                foreach (var child in followingItems)
                {
                    child.UpdateTitle(item.Title,
                                      item.TextType,
                                      item.Html?.Html,
                                      item.UserId);
                }
                await _repository.UnitOfWork.SaveEntitiesAsync();

            } while (count >= 10);
        }
    }
}
