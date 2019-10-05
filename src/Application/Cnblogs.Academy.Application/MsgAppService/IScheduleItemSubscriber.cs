using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.MsgAppService
{
    public interface IScheduleItemSubscriber
    {
        Task HandleScheduleItemCompletedEvent(ScheduleItemCompletedEvent e);

        Task HandleChildScheduleUpdatedEvent(ScheduleItemDeletedEvent e);

        Task HandleScheduleItemDispatchedEvent(ScheduleItemDispatchedEvent e);

        Task HandleChildScheduleItemUpdatedEvent(ChildScheduleItemUpdatedEvent e);
    }
}
