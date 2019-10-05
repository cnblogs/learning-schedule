using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class AddSummaryNoteCommandHandler : IRequestHandler<AddSummaryNoteCommand, long>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMarkdownApiService _markdwonSvc;

        public AddSummaryNoteCommandHandler(IScheduleRepository repository, IMarkdownApiService markdwonSvc)
        {
            _repository = repository;
            _markdwonSvc = markdwonSvc;
        }

        public async Task<long> Handle(AddSummaryNoteCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.ScheduleItems.Where(x => x.Id == request.ItemId).FirstOrDefaultAsync();
            if (item == null) return 0;
            var html = await _markdwonSvc.ToHtml(request.Note);
            var note = item.AddSummaryNote(request.ItemId, request.Note, html, request.UserId);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return note.Id;
        }
    }
}
