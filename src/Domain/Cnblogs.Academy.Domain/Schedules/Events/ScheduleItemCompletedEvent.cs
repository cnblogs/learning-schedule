using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleItemCompletedEvent)]
    public class ScheduleItemCompletedEvent : IDomainEvent
    {
        public ScheduleItemCompletedEvent(Guid itemUuid)
        {
            ItemUuid = itemUuid;
        }

        public Guid ItemUuid { get; }
    }

    [EventName(EventConst.ScheduleItemUndoEvent)]
    public class ScheduleItemUndoEvent : IDomainEvent
    {
        public ScheduleItemUndoEvent(long itemId, Guid userId)
        {
            ItemId = itemId;
            UserId = userId;
        }

        public long ItemId { get; }
        public Guid UserId { get; }
    }
}
