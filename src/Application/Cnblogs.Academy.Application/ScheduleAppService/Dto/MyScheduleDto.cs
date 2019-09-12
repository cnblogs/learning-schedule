using System;
using System.Collections.Generic;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Newtonsoft.Json;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class MyScheduleDto : RawScheduleDto
    {
        public IEnumerable<ScheduleItemDto> Items { get; set; }
    }
}
