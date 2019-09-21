using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Application.ProviderAppService
{
    public class MemberDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Photo { get; set; }

        [Required]
        public string Alias { get; set; }
        public string Remark { get; set; }
        public bool IsOwner { get; set; }
    }
}
