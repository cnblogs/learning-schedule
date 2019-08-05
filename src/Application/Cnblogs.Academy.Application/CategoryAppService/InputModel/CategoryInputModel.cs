using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.CategoryAppService.InputModel
{
    public class CategoryInputModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "分类名称不能超过20个字")]
        public string Name { get; set; }

        public long ParentId { get; set; }
    }
}
