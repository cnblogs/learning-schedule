using System;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class CommentPublishedEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long ObjectId { get; set; }
    }

    public class CommentDeletedEvent : CommentPublishedEvent
    {
    }
}
