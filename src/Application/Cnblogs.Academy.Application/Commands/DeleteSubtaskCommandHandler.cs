using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class DeleteSubtaskCommandHandler : IRequestHandler<DeleteSubtaskCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        public DeleteSubtaskCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteSubtaskCommand request, CancellationToken cancellationToken)
        {
            var subtask = await _repository.Subtasks.FirstOrDefaultAsync(x => x.Id == request.SubtaskId && x.UserId == request.UserId);
            subtask.Delete();
            return await _repository.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
