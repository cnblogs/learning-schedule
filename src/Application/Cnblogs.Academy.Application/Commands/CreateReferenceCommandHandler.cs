using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class CreateReferenceCommandHandler : IRequestHandler<CreateReferenceCommand, long>
    {
        private readonly IScheduleRepository _repository;
        public CreateReferenceCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(CreateReferenceCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.FirstOrDefaultAsync(x => x.Id == request.ItemId && x.UserId == request.UserId);
            if (item == null) throw new ValidationException("非法的请求");

            var reference = new Reference(request.Url, request.ItemId, request.UserId);
            item.AddReference(reference);
            await _repository.UnitOfWork.SaveChangesAsync();
            return reference.Id;
        }
    }
}
