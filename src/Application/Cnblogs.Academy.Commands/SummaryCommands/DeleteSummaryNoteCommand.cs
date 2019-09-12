using System;
using MediatR;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class DeleteSummaryNoteCommand : IRequest<bool>
    {
        public DeleteSummaryNoteCommand(long itemId, long noteId, Guid userId)
        {
            ItemId = itemId;
            NoteId = noteId;
            UserId = userId;
        }

        public long ItemId { get; }
        public long NoteId { get; }
        public Guid UserId { get; }
    }
}
