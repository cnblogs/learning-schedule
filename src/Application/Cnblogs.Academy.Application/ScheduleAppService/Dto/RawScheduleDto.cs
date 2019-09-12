using System;

namespace Cnblogs.Academy.Application.ScheduleAppService.Dto
{
    public class RawScheduleDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEnd { get; internal set; }
        public bool IsPrivate { get; set; }
    }
}
