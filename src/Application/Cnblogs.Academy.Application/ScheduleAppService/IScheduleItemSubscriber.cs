using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public interface IScheduleItemSubscriber
    {
        Task HandleScheduleItemCreatedEvent(ScheduleItemCreatedEvent e);

        Task HandleScheduleItemDeletedEvent(ScheduleItemDeletedEvent e);

        Task HandleScheduleItemUpdatedEvent(ScheduleItemUpdatedEvent e);
    }
}
