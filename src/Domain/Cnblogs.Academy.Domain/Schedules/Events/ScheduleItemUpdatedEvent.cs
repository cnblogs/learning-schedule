using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleItemUpdatedEvent)]
    public class ScheduleItemUpdatedEvent : IDomainEvent
    {

        public ScheduleItemUpdatedEvent(Guid itemUuid, string legacyTitle)
        {
            ItemUuid = itemUuid;
            LegacyTitle = legacyTitle;
        }

        public Guid ItemUuid { get; }
        public string LegacyTitle { get; }
    }

    [EventName(EventConst.ChildScheduleItemUpdatedEvent)]
    public class ChildScheduleItemUpdatedEvent : ScheduleItemUpdatedEvent
    {
        public ChildScheduleItemUpdatedEvent(Guid itemUuid, string legacyTitle) : base(itemUuid, legacyTitle)
        {
        }
    }
}
