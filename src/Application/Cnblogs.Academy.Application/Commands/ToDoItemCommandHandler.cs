using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Events;
using DotNetCore.CAP;
using Enyim.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class ToDoItemCommandHandler : IRequestHandler<ToDoItemCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;
        private readonly ICapPublisher _bus;
        private readonly IMemcachedClient _cache;

        public ToDoItemCommandHandler(IScheduleRepository scheduleRepository, ICapPublisher bus, IMemcachedClient cache)
        {
            _repository = scheduleRepository;
            _bus = bus;
            _cache = cache;
        }

        public async Task<BooleanResult> Handle(ToDoItemCommand request, CancellationToken cancellationToken)
        {
            var userId = request.User.UserId;
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == request.ScheduleId && s.UserId == userId);
            if (schedule == null) return BooleanResult.Fail("找不到对应的计划");
            ScheduleItem item = null;
            if (request.ItemId > 0)
            {
                item = await _repository.ScheduleItems.Include(i => i.Html).Where(i => i.Id == request.ItemId).FirstOrDefaultAsync();
            }

            var record = await _repository.Records.FirstOrDefaultAsync(r => r.UserId == userId && r.ItemId == request.ItemId);
            if (record == null)
            {
                record = schedule.ToDoItem(item, request.ScheduleId, userId, String.Empty, null);
                if (await _repository.UnitOfWork.SaveChangesAsync() > 0 && record != null)
                {
                    if (record != null)
                    {
                        await _bus.PublishAsync("ScheduleItem.Completed", new ScheduleItemCompletedEvent
                        {
                            Id = item.Id,
                            ItemUserId = item.UserId,
                            RecordId = record.Id,
                            UserId = userId,
                            Title = item.GenerateDescription(),
                            Link = $"{AppConst.DomainAddress}/schedules/u/{request.User.Alias}/{schedule.Id}/detail/{item.Id}?recordId={record.Id}",
                            IsPrivate = schedule.IsPrivate
                        });
                        await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                    }
                }
            }
            else if (item != null)
            {
                record.Update(String.Empty, null);
                await _repository.UnitOfWork.SaveChangesAsync();
            }
            return BooleanResult.Succeed();
        }
    }
}
