using System;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class RecordItemDto
    {
        public int SpaceUserId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
