using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules.Events;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public interface IScheduleSubscriber
    {
        Task HandleScheduleCompletedEvent(ScheduleCompletedEvent e);

        Task HandleScheduleCreatedEvent(ScheduleCreatedEvent e);

        Task HandleScheduleCancelledEvent(ScheduleCancelledEvent e);

        Task HandleScheduleDeletedEvent(ScheduleDeletedEvent e);

        Task HandleScheduleUpdatedEvent(ScheduleUpdatedEvent e);
    }
}
