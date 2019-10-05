using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleCompletedEvent)]
    public class ScheduleCompletedEvent : IDomainEvent
    {
        public Guid ScheduleUuid { get; }

        public ScheduleCompletedEvent(Guid scheduleUuid)
        {
            ScheduleUuid = scheduleUuid;
        }
    }
}
