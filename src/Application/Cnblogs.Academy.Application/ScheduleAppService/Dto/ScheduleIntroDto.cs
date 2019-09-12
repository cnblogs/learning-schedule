using System;
using Newtonsoft.Json;

namespace Cnblogs.Academy.Application.ScheduleAppService.Dto
{
    public class ScheduleIntroDto
    {
        public long Id { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
        
        public string UserName { get; set; }
        public string Alias { get; set; }
    }
}
