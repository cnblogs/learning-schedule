using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleInputModel
    {
        [Required(ErrorMessage = "学习计划名称不能为空")]
        [MinLength(2, ErrorMessage = "学习计划名称不能少于2个字")]
        [MaxLength(200, ErrorMessage = "学习计划名称不能超过200个字符")]
        public string Title { get; set; }

        [MaxLength(2000, ErrorMessage = "描述不能超过2000个字符")]
        public string Description { get; set; }

        public bool IsPrivate { get; set; } = false;
    }
}
