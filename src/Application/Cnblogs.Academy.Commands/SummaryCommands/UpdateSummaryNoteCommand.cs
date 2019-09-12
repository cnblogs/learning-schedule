using System;
using MediatR;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class UpdateSummaryNoteCommand : IRequest<bool>
    {
        public UpdateSummaryNoteCommand(long itemId, long id, string note, Guid userId)
        {
            ItemId = itemId;
            Id = id;
            Note = note;
            UserId = userId;
        }

        public long ItemId { get; }
        public long Id { get; }
        public string Note { get; }
        public Guid UserId { get; }
    }
}
