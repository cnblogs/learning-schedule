using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class Schedule : DisabledDefault, IAggregateRoot
    {
        private Schedule()
        {
            Items = new List<ScheduleItem>();
            Following = new List<ScheduleFollowing>();
        }

        public Schedule(string title, string description, Guid userId, bool isPrivate = false) : this()
        {
            Title = title;
            Description = description;
            UserId = userId;
            IsPrivate = isPrivate;
        }

        public long Id { get; private set; }

        [Required]
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset DateAdded { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset DateUpdated { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset? DateEnd { get; private set; }
        public ICollection<ScheduleItem> Items { get; private set; }
        public Stage Stage { get; private set; }

        public ICollection<ScheduleFollowing> Following { get; private set; }
        public int FollowingCount { get; private set; }

        [Timestamp]
        public byte[] Timestamp { get; private set; }

        public bool IsPrivate { get; private set; } = false;

        public ICollection<SchedulePrivateUpdateRecord> PrivateUpdateRecord { get; private set; }

        public (bool ok, string msg) AddFollowing(Guid userId)
        {
            if (userId == UserId)
            {
                return (false, "不能跟随自己的计划");
            }

            Following.Add(new ScheduleFollowing(Id, userId));
            FollowingCount += 1;
            return (true, null);
        }

        public ItemDoneRecord ToDoItem(ScheduleItem item, long id, Guid userId, string content,
            DateTimeOffset? dateDone)
        {
            if (item == null)
            {
                throw new ValidationException("无效的打卡请求");
            }

            if (UserId != userId)
            {
                if (item.Id < 1)
                    return null;
            }
            DateUpdated = DateTimeOffset.Now;
            var record = item.Done(userId, content, dateDone);
            return record;
        }

        public void Update(string title, string description, bool isPrivate = false)
        {
            Title = title;
            Description = description;
        }

        public void Delete()
        {
            foreach (var item in Items)
            {
                foreach (var record in item.Records)
                {
                    record.Delete();
                }
                item.Delete();
            }
            Deleted = true;
        }

        public void MarkAsComplete()
        {
            DateEnd = DateTimeOffset.Now;
            DateUpdated = DateTimeOffset.Now;
            Stage = Stage.Completed;
        }

        public void CancelComplete()
        {
            DateEnd = null;
            Stage = Stage.Started;
        }

        public void TogglePrivate(bool to)
        {
            IsPrivate = to;
            PrivateUpdateRecord = PrivateUpdateRecord ?? new List<SchedulePrivateUpdateRecord>();
            PrivateUpdateRecord.Add(new SchedulePrivateUpdateRecord(Id, to));
        }

        public void AddItem(ScheduleItem item)
        {
            Items = Items ?? new List<ScheduleItem>();
            Items.Add(item);
            DateUpdated = DateTimeOffset.Now;
            Stage = Stage.Started;
        }

        public void DeleteItems()
        {
            foreach (var item in Items)
            {
                item.Delete();
            }
        }
    }

    [Flags]
    public enum Status
    {
        Normal = 0,
        Disable = 1
    }

    public enum Stage
    {
        Starting,
        Started,
        Completed
    }
}
