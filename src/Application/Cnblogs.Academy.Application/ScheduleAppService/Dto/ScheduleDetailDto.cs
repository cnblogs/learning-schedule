using System.Collections.Generic;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.Application.ScheduleAppService.Dto
{
    public class ScheduleDetailDto : ScheduleDto
    {
        public IEnumerable<ScheduleItemDto> Items { get; set; }
    }
}
