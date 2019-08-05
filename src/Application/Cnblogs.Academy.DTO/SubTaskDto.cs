using System;

namespace Cnblogs.Academy.DTO
{
    public class SubtaskDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public long PreviousId { get; set; }
    }
}
