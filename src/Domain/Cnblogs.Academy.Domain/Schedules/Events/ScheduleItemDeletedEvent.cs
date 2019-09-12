using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleItemDeletedEvent)]
    public class ScheduleItemDeletedEvent : IDomainEvent
    {
        public Guid ItemUuid { get; }

        public ScheduleItemDeletedEvent(Guid itemUuid)
        {
            ItemUuid = itemUuid;
        }
    }

    [EventName(EventConst.ChildScheduleItemDeletedEvent)]
    public class ChildScheduleItemDeletedEvent : ScheduleItemDeletedEvent
    {
        public ChildScheduleItemDeletedEvent(Guid itemUuid) : base(itemUuid)
        {
        }
    }
}
