using System;

namespace Cnblogs.Academy.DTO
{
    public class ReferenceDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public DateTimeOffset DateAdded { get; set; }
    }
}
