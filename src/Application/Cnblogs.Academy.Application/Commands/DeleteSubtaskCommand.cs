using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class DeleteSubtaskCommand : IRequest<bool>
    {
        public long SubtaskId { get; set; }
        public Guid UserId { get; set; }

        public DeleteSubtaskCommand(long subtaskId, Guid userId)
        {
            UserId = userId;
            SubtaskId = subtaskId;
        }
    }
}
