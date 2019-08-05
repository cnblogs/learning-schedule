using System.Collections.Generic;

namespace Cnblogs.Academy.DTO
{
    public class ScheduleItemDetailDto : ScheduleItemDto
    {
        public IEnumerable<SubtaskDto> Subtasks { get; set; }
        public IEnumerable<ReferenceDto> References { get; set; }
        public IEnumerable<FeedbackDto> Feedbacks { get; set; }
    }
}
