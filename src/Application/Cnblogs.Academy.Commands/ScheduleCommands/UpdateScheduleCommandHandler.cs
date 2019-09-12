using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, bool>
    {
        private readonly IScheduleRepository _repository;

        public UpdateScheduleCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId);
            if (schedule != null)
            {
                schedule.Update(request.Model.Title, request.Model.Description, request.Model.IsPrivate, request.UserId);
                await _repository.UnitOfWork.SaveEntitiesAsync();
            }
            return true;
        }
    }
}
