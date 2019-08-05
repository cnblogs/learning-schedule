using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleItemMarkdownInput
    {
        [Required(ErrorMessage = "请输入学习任务")]
        [MaxLength(5000, ErrorMessage = "学习任务不能超过5000个字符")]
        public string Title { get; set; }
    }
}
