using System;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class ScheduleFollowing : DisabledDefault
    {
        public ScheduleFollowing()
        {
        }

        public ScheduleFollowing(long id, Guid userId)
        {
            ScheduleId = id;
            UserId = userId;
        }

        public long Id { get; set; }
        public long ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public Schedule Schedule { get; set; }

        public override void Disable()
        {
            base.Disable();
            // Schedule.FollowingCount -= 1;
        }

        public override void Restore()
        {
            base.Restore();
            // Schedule.FollowingCount += 1;
        }
    }
}
