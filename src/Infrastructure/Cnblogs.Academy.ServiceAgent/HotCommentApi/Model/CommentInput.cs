using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class CommentInput
    {
        [Required]
        [MaxLength(140, ErrorMessage = "评论内容不能超过140个字")]
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public string ObjectId { get; set; }
        public Guid? ParentId { get; set; }
        public string Ip { get; set; }
    }
}
