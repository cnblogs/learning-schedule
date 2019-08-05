using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Commands
{
    public class DeleteReferenceCommandHandler : IRequestHandler<DeleteReferenceCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        public DeleteReferenceCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteReferenceCommand request, CancellationToken cancellationToken)
        {
            var rf = await _repository.References.FirstOrDefaultAsync(x => x.Id == request.RefId && x.UserId == request.UserId);
            if (rf == null) throw new ValidationException("找不到要删除的链接");

            rf.Delete();
            return await _repository.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
