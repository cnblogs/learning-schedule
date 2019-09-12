using System;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, BooleanResult>
    {
        private readonly IScheduleRepository _repository;

        public CreateScheduleCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<BooleanResult> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            var im = request.Model;
            var schedule = new Schedule(im.Title, im.Description, request.User.UserId, im.IsPrivate);
            _repository.AddSchedule(schedule);
            if (await _repository.UnitOfWork.SaveEntitiesAsync())
            {
                return BooleanResult.Succeed(schedule.Id.ToString());
            }
            else
            {
                return BooleanResult.Fail("保存失败，请稍后重试");
            }
        }
    }
}
