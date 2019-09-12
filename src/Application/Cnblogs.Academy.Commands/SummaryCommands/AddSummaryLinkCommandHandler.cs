using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class AddSummaryLinkCommandHandler : IRequestHandler<AddSummaryLinkCommand, long>
    {
        private readonly IScheduleRepository _repository;

        public AddSummaryLinkCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<long> Handle(AddSummaryLinkCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems
                                        .Include(x => x.SummaryLinks)
                                        .Where(x => x.Id == request.ItemId)
                                        .FirstOrDefaultAsync();
            if (item == null) return 0;

            var link = item.AddSummaryLink(request.UserId, request.LinkDto.PostId, request.LinkDto.Title, request.LinkDto.Link);
            if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
            {
                return link.Id;
            }
            else
            {
                return 0;
            }
        }
    }
}
