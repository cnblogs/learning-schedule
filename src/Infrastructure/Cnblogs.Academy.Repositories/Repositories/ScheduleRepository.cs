using System;
using System.Linq;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Domain.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Repositories.Repositories
{
    public class ScheduleRepository : Repository<AcademyContext, Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AcademyContext context) : base(context)
        {
        }

        public IQueryable<Schedule> Schedules => _context.Schedules;

        public IQueryable<ScheduleItem> ScheduleItems => _context.ScheduleItems;

        public IQueryable<ItemDoneRecord> Records => _context.Records;

        public IQueryable<ScheduleFollowing> ScheduleFollowing => _context.Following;

        public IQueryable<SchedulePrivateUpdateRecord> PrivateUpdateRecord => _context.SchedulePrivateUpdateRecord;

        public IQueryable<Subtask> Subtasks => _context.Subtasks;

        public IQueryable<Reference> References => _context.References;

        public IQueryable<Feedback> Feedbacks => _context.Feedbacks;

        public void AddSchedule(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
        }

        public void AddScheduleItems(params ScheduleItem[] items)
        {
            foreach (var item in items)
            {
                if (_context.Entry<ScheduleItem>(item).State == EntityState.Detached)
                {
                    _context.ScheduleItems.Add(item);
                }
            }
        }

        public IQueryable<Schedule> GetWithUserId(long id, Guid uId)
        {
            var query = Schedules.Where(s => s.Id == id);
            if (uId != default(Guid))
            {
                query = query.Where(s => s.UserId == uId || s.Following.Any(f => f.UserId == uId));
            }
            return query;
        }
    }
}
