using System;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class ThumbupEvent
    {
        public string Id { get; set; }
        public long ObjectId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ThumbupCancellationEvent : ThumbupEvent
    {
    }
}
