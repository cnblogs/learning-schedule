using System;
using System.Linq;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Schedules
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        IQueryable<Schedule> Schedules { get; }
        IQueryable<ScheduleItem> ScheduleItems { get; }
        IQueryable<ItemDoneRecord> Records { get; }
        IQueryable<ScheduleFollowing> ScheduleFollowing { get; }
        IQueryable<SchedulePrivateUpdateRecord> PrivateUpdateRecord { get; }
        IQueryable<Subtask> Subtasks { get; }
        IQueryable<Reference> References { get; }
        IQueryable<Feedback> Feedbacks { get; }

        void AddSchedule(Schedule schedule);
        void AddScheduleItems(params ScheduleItem[] items);
        IQueryable<Schedule> GetWithUserId(long id, Guid userId);
    }
}
