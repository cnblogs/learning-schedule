using System;
using MediatR;

namespace Cnblogs.Academy.Application.Commands
{
    public class UndoItemCommand : IRequest<BooleanResult>
    {
        public long ScheduleId { get; set; }
        public long ItemId { get; set; }
        public Guid UserId { get; set; }

        public UndoItemCommand(long scheduleId, long itemId, Guid userId)
        {
            ScheduleId = scheduleId;
            ItemId = itemId;
            UserId = userId;
        }
    }
}
