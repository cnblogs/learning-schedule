using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class DeleteSummaryNoteCommandHandler : IRequestHandler<DeleteSummaryNoteCommand, bool>
    {
        private readonly IScheduleRepository _repository;

        public DeleteSummaryNoteCommandHandler(IScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteSummaryNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await _repository.SummaryNotes
                                        .Where(x => x.Id == request.NoteId)
                                        .FirstOrDefaultAsync();
            if (note != null)
            {
                note.Delete(request.UserId, request.ItemId);
                return await _repository.UnitOfWork.SaveChangesAsync(cancellationToken) > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
