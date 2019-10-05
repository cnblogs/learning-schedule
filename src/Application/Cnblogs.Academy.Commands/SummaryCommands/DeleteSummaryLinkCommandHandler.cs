using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class DeleteSummaryLinkCommandHandler:IRequestHandler<DeleteSummaryLinkCommand,bool>
    {
        private readonly IScheduleRepository _repository;

        public DeleteSummaryLinkCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteSummaryLinkCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems
                                        .Include(x => x.SummaryLinks)
                                        .Where(x => x.Id == request.ItemId)
                                        .FirstOrDefaultAsync();
            if (item == null)
                return true;

            item.RemoveSummaryLink(request.UserId, request.LinkId);
            return await _repository.UnitOfWork.SaveChangesAsync() > 0;
        }
    }
}
