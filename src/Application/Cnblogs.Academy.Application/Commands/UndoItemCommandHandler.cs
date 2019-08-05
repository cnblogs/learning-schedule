using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Events;
using DotNetCore.CAP;
using Enyim.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class UndoItemCommandHandler : IRequestHandler<UndoItemCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;
        private readonly ICapPublisher _bus;
        private readonly IMemcachedClient _cache;

        public UndoItemCommandHandler(IScheduleRepository repository, ICapPublisher bus, IMemcachedClient cache)
        {
            _repository = repository;
            _bus = bus;
            _cache = cache;
        }

        public async Task<BooleanResult> Handle(UndoItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.Include(x => x.Records)
                                        .Where(x => x.Id == request.ItemId && x.ScheduleId == request.ScheduleId && x.Schedule.UserId == request.UserId)
                                        .FirstOrDefaultAsync();

            if (item == null)
            {
                throw new ValidationException("无效的取消打卡请求");
            }
            else
            {
                item.Undo(request.UserId);
            }

            using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
            {
                await _repository.UnitOfWork.SaveChangesAsync();
                await _bus.PublishAsync("ScheduleItem.Completed.Canceled", new ScheduleItemCompletedCanceledEvent
                {
                    Id = item.Id,
                    UserId = item.UserId
                });
            }
            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            return BooleanResult.Succeed();
        }
    }
}
