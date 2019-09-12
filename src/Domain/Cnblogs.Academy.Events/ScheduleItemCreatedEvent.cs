using System;

namespace Cnblogs.Academy.Events
{
    public class ScheduleItemCreatedEvent : ScheduleCreatedEvent
    {
        /// <summary>
        /// 计划创建者的ID
        /// </summary>
        /// <value></value>
        public Guid ScheduleUserId { get; set; }
    }

    public class ScheduleItemCompletedEvent : ScheduleCreatedEvent
    {
        /// <summary>
        /// 任务的制定者的ID
        /// </summary>
        /// <value></value>
        public Guid ItemUserId { get; set; }
        public long RecordId { get; set; }
    }

    public class ScheduleItemDeletedEvent : ScheduleDeletedEvent { }

    public class ScheduleItemUpdatedEvent : ScheduleUpdatedEvent { }

    public class ScheduleItemCompletedCanceledEvent : ScheduleCompletedCanceledEvent { }
}
