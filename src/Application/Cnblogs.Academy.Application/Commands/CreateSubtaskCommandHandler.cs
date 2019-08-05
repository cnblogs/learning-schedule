using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class CreateSubtaskCommandHandler : IRequestHandler<CreateSubtaskCommand, long>
    {
        private readonly IScheduleRepository _repository;

        public CreateSubtaskCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(CreateSubtaskCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.FirstOrDefaultAsync(x => x.Id == request.ItemId && x.UserId == request.UserId);
            if (item == null) throw new ValidationException("非法的请求");

            var subtask = new Subtask(request.ItemId, request.Content, request.UserId);
            item.AddSubtask(subtask);
            await _repository.UnitOfWork.SaveChangesAsync();
            return subtask.Id;
        }
    }
}
