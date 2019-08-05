using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class Subtask
    {
        private Subtask()
        {
        }

        public Subtask(long itemId, string content, Guid userId)
        {
            ItemId = itemId;
            Content = content;
            UserId = userId;
        }

        public long Id { get; private set; }
        public long ItemId { get; private set; }
        public ScheduleItem Item { get; private set; }

        [MaxLength(500)]
        public string Content { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset DateAdded { get; private set; }
        public DateTimeOffset? DateEnd { get; private set; }

        public long PreviousId { get; private set; }
        public bool Deleted { get; private set; }

        public void Accomplish(long itemId, Guid userId, bool completed)
        {
            if (ItemId != itemId)
            {
                throw new ValidationException("非法的学习任务");
            }
            if (Item.Schedule.UserId != userId)
            {
                throw new ValidationException("只有发布学习目标的人才能完成子任务");
            }
            if (completed)
            {
                DateEnd = DateTimeOffset.Now;
            }
            else
            {
                DateEnd = null;
            }
        }

        public void Update(string content)
        {
            Content = content;
        }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
