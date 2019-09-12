using System.Collections.Generic;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.Application.ScheduleAppService.Dto
{
    public class ScheduleDetailDto : ScheduleDto
    {
        public long? ParentId { get; set; }
        public ScheduleIntroDto Parent { get; set; }
        public IEnumerable<ScheduleItemDto> Items { get; set; }
    }
}
