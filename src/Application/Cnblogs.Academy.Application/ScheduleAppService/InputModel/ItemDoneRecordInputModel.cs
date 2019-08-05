using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ItemDoneRecordInputModel
    {
        [MaxLength(500000, ErrorMessage = "内容不能超过50万个字符")]
        public DateTimeOffset? DateDone { get; set; }
        public string Content { get; set; }
    }
}
