using Cnblogs.Academy.Domain.Schedules;

namespace Cnblogs.Academy.DTO.InputModel
{
    public class FeedbackInputModel
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Content { get; set; }
    }
}
