using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.ScheduleCancelledEvent)]
    public class ScheduleCancelledEvent : IDomainEvent
    {

        public ScheduleCancelledEvent(Guid scheduleUuid)
        {
            ScheduleUuid = scheduleUuid;
        }

        public Guid ScheduleUuid { get; }
    }
}
