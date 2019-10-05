namespace Cnblogs.Academy.Domain.Events
{
    public static class EventConst
    {
        public const string NewSubscriber = "Academy.NewSubscriber";
        public const string UnsubscribeEvent = "Academy.Unsubscribe";
        public const string ScheduleCancelledEvent = "Academy.Schedule.Cancellated";
        public const string ScheduleCompletedEvent = "Academy.Schedule.Completed";
        public const string ScheduleCreatedEvent = "Academy.Schedule.Created";
        public const string ScheduleDeletedEvent = "Academy.Schedule.Deleted";
        public const string ScheduleUpdatedEvent = "Academy.Schedule.Updated";

        public const string ChildScheduleUpdatedEvent = "Academy.ChildSchedule.Updated";

        public const string ScheduleItemCompletedEvent = "Academy.ScheduleItem.Completed";
        public const string ScheduleItemUndoEvent = "Academy.ScheduleItem.Undo";
        public const string ScheduleItemCreatedEvent = "Academy.ScheduleItem.Created";
        public const string ScheduleItemDispatchedEvent = "Academy.ScheduleItem.Dispatched";
        public const string ScheduleItemDeletedEvent = "Academy.ScheduleItem.Deleted";
        public const string ScheduleItemUpdatedEvent = "Academy.ScheduleItem.Updated";

        public const string ChildScheduleItemDeletedEvent = "Academy.ChildScheduleItem.Deleted";
        public const string ChildScheduleItemUpdatedEvent = "Academy.ChildScheduleItem.Updated";

    }
}
