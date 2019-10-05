using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.DTO.InputModel
{
    public class SummaryNoteInputModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "感想不能为空")]
        public string Note { get; set; }
    }
}
