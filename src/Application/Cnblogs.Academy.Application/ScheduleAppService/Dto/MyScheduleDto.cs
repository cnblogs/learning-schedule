using System.Collections.Generic;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class MyScheduleDto : RawScheduleDto
    {
        public IEnumerable<ScheduleItemDto> Items { get; set; }
    }
}
