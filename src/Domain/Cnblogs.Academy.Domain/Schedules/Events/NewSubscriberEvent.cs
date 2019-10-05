using System;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Domain.Schedules.Events
{
    [EventName(EventConst.NewSubscriber)]
    public class NewSubscriberEvent : IDomainEvent
    {
        public NewSubscriberEvent(Guid parentScheduleUuid, Guid childScheduleUuid)
        {
            ParentScheduleUuid = parentScheduleUuid;
            ChildScheduleUuid = childScheduleUuid;
        }

        public Guid ParentScheduleUuid { get; }
        public Guid ChildScheduleUuid { get; }
    }

    [EventName(EventConst.UnsubscribeEvent)]
    public class UnsubscribeEvent : IDomainEvent
    {
        public long ParentId { get; set; }
        public UnsubscribeEvent(long parentId)
        {
            ParentId = parentId;
        }
    }
}
