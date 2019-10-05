using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.MsgAppService
{
    public interface IScheduleEventSubscriber
    {
        Task HandleNewSubscriberEvent(NewSubscriberEvent e);

        Task HandleScheduleCompletedEvent(ScheduleCompletedEvent e);

        Task HandleChildScheduleUpdatedEvent(ChildScheduleUpdatedEvent e);
    }
}
