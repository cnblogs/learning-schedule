using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class CancelScheduleCommandHandler : IRequestHandler<CancelScheduleCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;

        public CancelScheduleCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<BooleanResult> Handle(CancelScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _repository.Schedules
                                            .Where(s => s.UserId == request.User.UserId && s.Id == request.ScheduleId)
                                            .FirstOrDefaultAsync();

            if (schedule == null) return BooleanResult.Fail("找不到对应的目标");

            schedule.CancelComplete();
            await _repository.UnitOfWork.SaveEntitiesAsync();
            return BooleanResult.Succeed();
        }
    }
}
