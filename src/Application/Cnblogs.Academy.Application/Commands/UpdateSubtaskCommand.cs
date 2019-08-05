using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class UpdateSubtaskCommand : IRequest<bool>
    {
        public long ItemId { get; set; }
        public long SubtaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }

        public UpdateSubtaskCommand(long itemId, long subtaskId, Guid userId, string content)
        {
            Content = content;
            UserId = userId;
            SubtaskId = subtaskId;
            ItemId = itemId;
        }
    }
}
