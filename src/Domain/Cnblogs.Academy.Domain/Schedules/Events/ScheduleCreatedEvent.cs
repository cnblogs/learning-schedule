using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{

    [EventName(EventConst.ScheduleCreatedEvent)]
    public class ScheduleCreatedEvent : IDomainEvent
    {
        public ScheduleCreatedEvent(Guid scheduleUuid)
        {
            ScheduleUuid = scheduleUuid;
        }

        public Guid ScheduleUuid { get; }
    }
}
