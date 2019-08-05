using System;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class ItemDoneRecord : DisabledDefault
    {
        public ItemDoneRecord()
        {
        }

        public ItemDoneRecord(long itemId, Guid userId, string content, DateTimeOffset? doneTime)
        {
            ItemId = itemId;
            UserId = userId;
            Content = content;
            if (doneTime.HasValue)
            {
                DoneTime = doneTime.Value;
            }
        }

        public long Id { get; set; }
        public long ItemId { get; set; }
        public ScheduleItem Item { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset DoneTime { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }

        public void Update(string content, DateTimeOffset? dateDone = null)
        {
            Content = content;
            if (dateDone.HasValue)
            {
                DoneTime = dateDone.Value;
            }
            else
            {
                DoneTime = default(DateTimeOffset);
            }
        }

        public override void Disable()
        {
            base.Disable();
            Item.DoneCount -= 1;
        }

        public override void Restore()
        {
            base.Restore();
            Item.DoneCount += 1;
        }

        public void Delete()
        {
            Deleted = true;
            Item.DoneCount -= 1;
        }
    }
}
