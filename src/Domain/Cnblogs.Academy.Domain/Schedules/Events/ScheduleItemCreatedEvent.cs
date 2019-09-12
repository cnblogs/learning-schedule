using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleItemCreatedEvent)]
    public class ScheduleItemCreatedEvent : IDomainEvent
    {
        public ScheduleItemCreatedEvent(Guid itemUuid)
        {
            ItemUuid = itemUuid;
        }

        public Guid ItemUuid { get; }
    }

    [EventName(EventConst.ScheduleItemDispatchedEvent)]
    public class ScheduleItemDispatchedEvent : IDomainEvent
    {
        public Guid ChildItemUuid;

        public ScheduleItemDispatchedEvent(Guid childItemUuid)
        {
            ChildItemUuid = childItemUuid;
        }
    }
}
