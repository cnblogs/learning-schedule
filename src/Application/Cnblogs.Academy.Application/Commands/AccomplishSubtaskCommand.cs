using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class AccomplishSubtaskCommand : IRequest<bool>
    {
        public long ItemId { get; set; }
        public long SubtaskId { get; set; }
        public Guid UserId { get; set; }
        public bool Completed { get; set; }
        public AccomplishSubtaskCommand(long itemId, long subtaskId, Guid userId, bool completed)
        {
            Completed = completed;
            UserId = userId;
            SubtaskId = subtaskId;
            ItemId = itemId;
        }
    }
}
