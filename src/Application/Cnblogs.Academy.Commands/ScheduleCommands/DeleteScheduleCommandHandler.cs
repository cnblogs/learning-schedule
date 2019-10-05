using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ScheduleCommands
{
    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, bool>
    {
        private readonly IScheduleRepository _repository;

        public DeleteScheduleCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _repository.Schedules.Include(x => x.Items).Include(x => x.Parent)
                                    .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId);
            if (schedule != null)
            {
                schedule.Delete(request.UserId);
                return await _repository.UnitOfWork.SaveEntitiesAsync();
            }
            return true;
        }
    }
}
