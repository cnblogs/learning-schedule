using System;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class SummaryNote : Entity
    {

        public SummaryNote(long itemId, string note, string html, Guid userId)
        {
            ItemId = itemId;
            Note = note;
            Html = html;
            UserId = userId;
        }

        public long ItemId { get; private set; }
        public ScheduleItem Item { get; private set; }
        public string Note { get; private set; }
        public string Html { get; private set; }
        public Guid UserId { get; private set; }

        public void Update(string note, string html)
        {
            Note = note;
            Html = html;
        }

        public void Delete(Guid userId, long itemId)
        {
            if (UserId == userId && ItemId == itemId)
            {
                Deleted = true;
            }
        }
    }
}
