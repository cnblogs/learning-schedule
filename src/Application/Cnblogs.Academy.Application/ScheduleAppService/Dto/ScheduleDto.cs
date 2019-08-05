using System;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Newtonsoft.Json;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleDto : ScheduleInputModel
    {
        public long Id { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public Stage Stage { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        public AcademyUserDto User { get; set; }

        public ScheduleDto PatchUserInfo(UserDto user)
        {
            User = new AcademyUserDto(user);
            return this;
        }
    }
}
