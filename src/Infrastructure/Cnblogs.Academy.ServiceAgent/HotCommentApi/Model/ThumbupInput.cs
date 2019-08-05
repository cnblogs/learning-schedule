using System;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class ThumbupInput
    {
        public ThumbupInput(string objectId, string ip, Guid userId)
        {
            ObjectId = objectId;
            Ip = ip;
            UserId = userId;
        }

        public string ObjectId { get; set; }
        public string Ip { get; set; }
        public Guid UserId { get; set; }
    }

    public class ThumbCancellationInput : ThumbupInput
    {
        public ThumbCancellationInput(string objectId, string ip, Guid userId) : base(objectId, ip, userId)
        {
        }
    }
}
