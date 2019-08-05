using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class AccomplishSubtaskCommandHandler : IRequestHandler<AccomplishSubtaskCommand, bool>
    {
        private readonly IScheduleRepository _reposiotry;
        public AccomplishSubtaskCommandHandler(IScheduleRepository reposiotry)
        {
            _reposiotry = reposiotry;
        }

        public async Task<bool> Handle(AccomplishSubtaskCommand request, CancellationToken cancellationToken)
        {
            var subtask = await _reposiotry.Subtasks.Include(x => x.Item).ThenInclude(x => x.Schedule)
            .Where(x => x.Id == request.SubtaskId)
            .FirstOrDefaultAsync();

            if (subtask == null) throw new ValidationException("子任务不存在");

            subtask.Accomplish(request.ItemId, request.UserId, request.Completed);

            return await _reposiotry.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
