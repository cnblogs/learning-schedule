using System.Threading.Tasks;
using Cnblogs.Academy.Events;

namespace Cnblogs.Academy.Subscriber
{
    public interface IScheduleEventHandler
    {
        Task HandleScheduleItemCreated(ScheduleItemCreatedEvent e);

        Task HandleScheduleItemCompleted(ScheduleItemCompletedEvent e);
    }
}
