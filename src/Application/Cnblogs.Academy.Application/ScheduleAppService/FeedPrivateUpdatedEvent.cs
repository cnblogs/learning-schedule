using Cnblogs.Academy.Domain;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class FeedPrivateUpdatedEvent
    {
        public FeedPrivateUpdatedEvent(FeedType feedType, long contentId, bool isPrivate)
        {
            FeedType = feedType;
            ContentId = contentId;
            IsPrivate = isPrivate;
        }

        public FeedType FeedType { get; set; }
        public long ContentId { get; set; }
        public bool IsPrivate { get; set; }
    }
}
