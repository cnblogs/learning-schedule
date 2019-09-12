using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public interface IScheduleSubscriber
    {
        Task HandleScheduleUpdatedEvent(ScheduleUpdatedEvent e);

        Task HandleNewSubscriberEvent(NewSubscriberEvent e);

        Task HandleUnsubscribeEvent(UnsubscribeEvent e);
    }
}
