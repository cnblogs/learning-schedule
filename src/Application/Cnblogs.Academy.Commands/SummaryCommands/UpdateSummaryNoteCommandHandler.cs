using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class UpdateSummaryNoteCommandHandler : IRequestHandler<UpdateSummaryNoteCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMarkdownApiService _markdownSvc;

        public UpdateSummaryNoteCommandHandler(IScheduleRepository repository, IMarkdownApiService markdownSvc)
        {
            _repository = repository;
            _markdownSvc = markdownSvc;
        }

        public async Task<bool> Handle(UpdateSummaryNoteCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.Include(x => x.SummaryNote).Where(x => x.Id == request.ItemId).FirstOrDefaultAsync();
            if (item != null)
            {
                var html = await _markdownSvc.ToHtml(request.Note);
                item.UpdateSummaryNote(request.Note, html, request.UserId);
                return await _repository.UnitOfWork.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
