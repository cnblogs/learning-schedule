using System;
using Newtonsoft.Json;

namespace Cnblogs.Academy.DTO
{
    public class ScheduleFollowingDto
    {
        public long ScheduleId { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }

        public AcademyUserDto User { get; set; }
    }
}
