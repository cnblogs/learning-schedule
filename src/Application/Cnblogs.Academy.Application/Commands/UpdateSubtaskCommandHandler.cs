using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class UpdateSubtaskCommandHandler : IRequestHandler<UpdateSubtaskCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        public UpdateSubtaskCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateSubtaskCommand request, CancellationToken cancellationToken)
        {
            var subtask = await _repository.Subtasks.FirstOrDefaultAsync(x => x.Id == request.SubtaskId && x.UserId == request.UserId);
            if (subtask == null) throw new ValidationException("找不到要修改的子任务");
            subtask.Update(request.Content);
            return await _repository.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
