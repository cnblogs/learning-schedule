using System;
using MediatR;

namespace Cnblogs.Academy.Application.ItemCommands
{
    public class AddSummaryNoteCommand : IRequest<long>
    {
        public AddSummaryNoteCommand(long itemId, string note, Guid userId)
        {
            ItemId = itemId;
            Note = note;
            UserId = userId;
        }

        public long ItemId { get; }
        public string Note { get; }
        public Guid UserId { get; }
    }
}
