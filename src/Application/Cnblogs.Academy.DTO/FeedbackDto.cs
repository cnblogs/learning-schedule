using System;
using Cnblogs.Academy.Domain.Schedules;

namespace Cnblogs.Academy.DTO
{
    public class FeedbackDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Content { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
