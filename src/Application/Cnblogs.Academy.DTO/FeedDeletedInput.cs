using System;
using Cnblogs.Academy.Domain;

namespace Cnblogs.Academy.DTO
{
    public class FeedDeletedInput
    {
        public string ContentId { get; set; }
        public Guid AppId { get; set; }
        public Guid UserId { get; set; }
        public FeedType FeedType { get; set; }
    }
}