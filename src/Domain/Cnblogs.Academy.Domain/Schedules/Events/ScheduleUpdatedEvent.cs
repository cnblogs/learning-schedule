using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleUpdatedEvent)]
    public class ScheduleUpdatedEvent : IDomainEvent
    {
        public ScheduleUpdatedEvent(Guid scheduleUuid, string legacyTitle)
        {
            ScheduleUuid = scheduleUuid;
            LegacyTitle = legacyTitle;
        }

        public Guid ScheduleUuid { get; }
        public string LegacyTitle { get; }
    }

    [EventName(EventConst.ChildScheduleUpdatedEvent)]
    public class ChildScheduleUpdatedEvent : ScheduleUpdatedEvent
    {
        public ChildScheduleUpdatedEvent(Guid scheduleUuid, string legacyTitle) : base(scheduleUuid, legacyTitle)
        {
        }
    }
}
