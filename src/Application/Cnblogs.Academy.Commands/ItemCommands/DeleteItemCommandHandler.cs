using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly IScheduleRepository _repository;

        public DeleteItemCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.Include(x => x.Schedule)
                                        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            if (item != null)
            {
                item.Schedule.DeleteItem(request.Id, request.UserId);
                await _repository.UnitOfWork.SaveEntitiesAsync();
            }
            return true;
        }
    }
}
