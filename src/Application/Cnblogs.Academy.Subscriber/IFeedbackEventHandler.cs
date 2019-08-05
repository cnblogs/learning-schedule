using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Events;

namespace Cnblogs.Academy.Subscriber
{
    public interface IFeedbackEventHandler
    {
        Task HandleFeedbackCreatedEvent(FeedbackCreatedEvent e);
    }
}
