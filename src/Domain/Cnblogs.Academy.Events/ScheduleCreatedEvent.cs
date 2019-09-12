using System;

namespace Cnblogs.Academy.Events
{
    public class ScheduleCreatedEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Guid UserId { get; set; }
        public bool IsPrivate { get; set; }
    }

    public class ScheduleCompletedEvent : ScheduleCreatedEvent
    {
    }

    public class ScheduleDeletedEvent
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class ScheduleUpdatedEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }

    public class ScheduleCompletedCanceledEvent
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
    }

    public class SchedulePrivateUpdatedEvent
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long[] ItemIds { get; set; }
        public bool IsPrivate { get; set; }
    }
}
