using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class CompleteItemCommandHandler : IRequestHandler<CompleteItemCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;

        public CompleteItemCommandHandler(IScheduleRepository scheduleRepository)
        {
            _repository = scheduleRepository;
        }

        public async Task<BooleanResult> Handle(CompleteItemCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == request.ScheduleId && s.UserId == userId);
            if (schedule == null) return BooleanResult.Fail("找不到对应的计划");
            if (request.ItemId > 0)
            {
                var item = await _repository.ScheduleItems.Include(i => i.Html).Where(i => i.Id == request.ItemId).FirstOrDefaultAsync();

                schedule.CompleteItem(item, userId);

                await _repository.UnitOfWork.SaveEntitiesAsync();
            }

            return BooleanResult.Succeed();
        }
    }
}
