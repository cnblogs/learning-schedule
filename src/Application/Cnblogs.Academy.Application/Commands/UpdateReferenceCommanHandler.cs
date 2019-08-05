using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class UpdateReferenceCommanHandler : IRequestHandler<UpdateReferenceCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        public UpdateReferenceCommanHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateReferenceCommand request, CancellationToken cancellationToken)
        {
            var reference = await _repository.References.FirstOrDefaultAsync(x => x.Id == request.RefId && x.UserId == request.UserId);
            if (reference == null) throw new ValidationException("找不到要修改的链接");

            reference.Update(request.Url);
            return await _repository.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
