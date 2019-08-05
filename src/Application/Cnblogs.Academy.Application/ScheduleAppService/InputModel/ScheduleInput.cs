using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleInputModel
    {
        [Required(ErrorMessage = "请输入学习目标")]
        [MinLength(2, ErrorMessage = "学习目标不能少于2个字")]
        [MaxLength(200, ErrorMessage = "学习目标不能超过200个字符")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "描述不能超过2000个字符")]
        public string Description { get; set; }

        public bool IsPrivate { get; set; } = false;
    }
}
