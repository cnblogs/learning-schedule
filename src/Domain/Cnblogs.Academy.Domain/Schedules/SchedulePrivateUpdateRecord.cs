using System;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class SchedulePrivateUpdateRecord
    {
        private SchedulePrivateUpdateRecord()
        {
        }

        public SchedulePrivateUpdateRecord(long scheduleId, bool isPrivate)
        {
            ScheduleId = scheduleId;
            IsPrivate = isPrivate;
        }

        public long Id { get; private set; }
        public long ScheduleId { get; private set; }
        public Schedule Schedule { get; private set; }
        public bool IsPrivate { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }
    }
}
