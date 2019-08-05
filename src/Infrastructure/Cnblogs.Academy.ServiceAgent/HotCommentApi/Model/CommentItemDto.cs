using System;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class CommentItem
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public string ObjectId { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }
        public int SpaceuserId { get; set; }

        public Guid? ParentId { get; set; }

        public string Ip { get; set; }

        public string UserName { get; set; }

        public string HomeUrl { get; set; }

    }
}
