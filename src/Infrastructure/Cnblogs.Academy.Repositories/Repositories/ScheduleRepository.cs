using System;
using System.Linq;
using Cnblogs.Academy.Domain;
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

        public IQueryable<SchedulePrivateUpdateRecord> PrivateUpdateRecord => _context.SchedulePrivateUpdateRecord;

        public IQueryable<SummaryNote> SummaryNotes => _context.SummaryNotes;

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

        public IQueryable<T> FindByUUID<T>(Guid uuid, bool tracking = false) where T : BaseEntity
        {
            IQueryable<T> query = _context.Set<T>();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query.Where(x => x.UUID == uuid);
        }
    }
}
