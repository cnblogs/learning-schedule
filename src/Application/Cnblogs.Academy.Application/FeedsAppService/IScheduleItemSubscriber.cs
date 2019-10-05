using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public interface IScheduleItemSubscriber
    {
        Task HandleScheduleItemCompletedEvent(ScheduleItemCompletedEvent e);

        Task HandleScheduleItemCreatedEvent(ScheduleItemCreatedEvent e);

        Task HandleScheduleItemDeletedEvent(ScheduleItemDeletedEvent e);

        Task HandleScheduleItemUndoEvent(ScheduleItemUndoEvent e);

        Task HandleScheduleItemUpdatedEvent(ScheduleItemUpdatedEvent e);
    }
}
