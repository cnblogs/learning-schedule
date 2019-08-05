using System.ComponentModel;
using Cnblogs.Academy.Domain.Schedules;

namespace Cnblogs.Academy.Domain.Events
{
    [Description("ScheduleItem.Feedback.Created")]
    public class FeedbackCreatedEvent : IDomainEvent
    {
        public Feedback Feedback { get; set; }

        public FeedbackCreatedEvent(Feedback feedback)
        {
            Feedback = feedback;
        }
    }

    [Description("ScheduleItem.Feedback.Updated")]
    public class FeedbackUpdatedEvent : FeedbackCreatedEvent
    {
        public FeedbackUpdatedEvent(Feedback feedback) : base(feedback)
        {
        }
    }
}
