using Cnblogs.Academy.ServiceAgent.HotCommentApi;

namespace Cnblogs.Academy.WebAPI.Model
{
    public class CommentItemDto : CommentItem
    {
        public bool Deletable { get; set; }
        public bool Replyable { get; set; }
    }
}
