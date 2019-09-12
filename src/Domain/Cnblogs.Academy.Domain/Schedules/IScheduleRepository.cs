using System;
using System.Linq;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Schedules
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        IQueryable<Schedule> Schedules { get; }
        IQueryable<ScheduleItem> ScheduleItems { get; }
        IQueryable<SchedulePrivateUpdateRecord> PrivateUpdateRecord { get; }
        IQueryable<SummaryNote> SummaryNotes { get; }

        void AddSchedule(Schedule schedule);
        void AddScheduleItems(params ScheduleItem[] items);
        IQueryable<T> FindByUUID<T>(Guid uuid, bool tracking = false) where T : BaseEntity;
    }
}
