using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class Reference
    {
        private Reference()
        {
        }

        public Reference(string url, long itemId, Guid userId)
        {
            Url = url;
            ItemId = itemId;
            UserId = userId;
        }

        public long Id { get; private set; }

        [MaxLength(500)]
        public string Url { get; private set; }
        public long ItemId { get; private set; }
        public ScheduleItem Item { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }
        public bool Deleted { get; private set; }
        public Guid UserId { get; set; }

        public void Delete()
        {
            Deleted = true;
        }

        public void Update(string url)
        {
            Url = url;
        }
    }
}
