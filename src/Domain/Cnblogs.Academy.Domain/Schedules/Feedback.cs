using System;
using System.ComponentModel.DataAnnotations;
using Cnblogs.Academy.Domain.Events;
using Newtonsoft.Json;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class Feedback : Entity
    {
        private Feedback()
        {
        }

        public Feedback(long itemId, Difficulty difficulty, string content, Guid userId)
        {
            ItemId = itemId;
            Difficulty = difficulty;
            Content = content;
            UserId = userId;
            AddDomainEvent(new FeedbackCreatedEvent(this));
        }

        public long ItemId { get; private set; }
        [JsonIgnore]
        public ScheduleItem Item { get; private set; }
        public Difficulty Difficulty { get; private set; }

        [MaxLength(500)]
        public string Content { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }
        public Guid UserId { get; private set; }
        public bool Deleted { get; private set; }

        public Feedback Update(Difficulty difficulty, string content, Guid userId)
        {
            if (UserId == userId)
            {
                Difficulty = difficulty;
                Content = content;
                AddDomainEvent(new FeedbackUpdatedEvent(this));
            }
            return this;
        }
    }
}
