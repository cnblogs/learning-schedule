using Cnblogs.Academy.Domain;

namespace Cnblogs.Academy.DTO
{
    public class FeedUpdateModel
    {
        public string ContentId { get; set; }
        public string FeedTitle { get; set; }
        public FeedType FeedType { get; set; }
        public string FeedContent { get; set; }
    }
}