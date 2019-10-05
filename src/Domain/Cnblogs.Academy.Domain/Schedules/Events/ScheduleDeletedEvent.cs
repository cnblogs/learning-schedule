using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleDeletedEvent)]
    public class ScheduleDeletedEvent : IDomainEvent
    {
        public ScheduleDeletedEvent(Guid scheduleUuid)
        {
            ScheduleUuid = scheduleUuid;
        }

        public Guid ScheduleUuid { get; }
    }
}
