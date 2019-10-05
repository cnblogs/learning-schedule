using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cnblogs.Academy.Domain.Schedules.Events;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class Schedule : Entity, IAggregateRoot
    {
        private Schedule()
        {
            Items = new List<ScheduleItem>();
            Children = new List<Schedule>();
        }

        public Schedule(string title, string description, Guid userId, bool isPrivate = false, long? parentId = null) : this()
        {
            Title = title;
            Description = description;
            UserId = userId;
            IsPrivate = isPrivate;
            if (parentId.HasValue)
            {
                ParentId = parentId;
            }
            AddDomainEvent(new ScheduleCreatedEvent(UUID));
        }

        [Required]
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid UserId { get; private set; }
        public DateTimeOffset DateUpdated { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset? DateEnd { get; private set; }
        public ICollection<ScheduleItem> Items { get; private set; }
        public Stage Stage { get; private set; }

        public int FollowingCount { get; private set; }
        public long? ParentId { get; private set; }
        public Schedule Parent { get; private set; }
        public ICollection<Schedule> Children { get; private set; }

        [Timestamp]
        public byte[] Timestamp { get; private set; }

        public bool IsPrivate { get; private set; } = false;

        public ICollection<SchedulePrivateUpdateRecord> PrivateUpdateRecord { get; private set; }

        public Schedule Subscribe(Guid userId)
        {
            if (userId == UserId)
            {
                throw new ValidationException("不能借鉴自己的计划");
            }

            if (ParentId > 0)
            {
                throw new ValidationException("很抱歉，暂时不支持借鉴来的学习计划");
            }
            var child = Deliver(userId);
            Children.Add(child);
            FollowingCount += 1;

            AddDomainEvent(new NewSubscriberEvent(this.UUID, child.UUID));

            return child;
        }

        public void AcceptItemsFrom(List<ScheduleItem> parentItems)
        {
            var sortedItems = parentItems.OrderBy(x => x.DateAdded);
            foreach (var item in sortedItems)
            {
                AddItem(item.Deliver(Id, UserId));
            }
        }

        public Schedule Deliver(Guid userId)
        {
            var schedule = new Schedule(Title, Description, userId, isPrivate: false, Id);
            return schedule;
        }

        public void UnSubscribe()
        {
            if (ParentId > 0)
            {
                AddDomainEvent(new UnsubscribeEvent(ParentId.Value));
            }
            if (Parent != null)
                Parent.FollowingCount -= 1;
        }

        public void CompleteItem(ScheduleItem item, Guid userId, DateTimeOffset? dateDone = default)
        {
            if (item == null)
            {
                throw new ValidationException("无效的打卡请求");
            }

            if (UserId != userId)
            {
                if (item.Id < 1) return;
            }

            item.Complete(userId, dateDone);
            DateUpdated = DateTimeOffset.Now;
            AddDomainEvent(new ScheduleItemCompletedEvent(item.UUID));
        }

        public void Update(string title, string description, bool isPrivate = false, Guid? operatorId = null)
        {
            if (ParentId > 0)
            {
                if (operatorId == UserId)
                    throw new ValidationException("很抱歉，不能修改借鉴来的学习计划");
            }
            var legacyTitle = Title;
            Title = title;
            Description = description;
            if (ParentId > 0)
                AddDomainEvent(new ChildScheduleUpdatedEvent(UUID, legacyTitle));
            else
                AddDomainEvent(new ScheduleUpdatedEvent(UUID, legacyTitle));
        }

        public void Delete(Guid operatorId = default(Guid))
        {
            if (FollowingCount > 0)
            {
                throw new ValidationException("该计划已经有同学借鉴了，不能删除了");
            }
            foreach (var item in Items)
            {
                DeleteItem(item.Id);
            }
            UnSubscribe();
            Deleted = true;
            AddDomainEvent(new ScheduleDeletedEvent(UUID));
        }

        public void MarkAsComplete()
        {
            DateEnd = DateTimeOffset.Now;
            DateUpdated = DateTimeOffset.Now;
            Stage = Stage.Completed;
            AddDomainEvent(new ScheduleCompletedEvent(UUID));
        }

        public void CancelComplete()
        {
            DateEnd = null;
            Stage = Stage.Started;
            AddDomainEvent(new ScheduleCancelledEvent(UUID));
        }

        public void TogglePrivate(bool to)
        {
            if (to)
            {
                if (FollowingCount > 0)
                    throw new ValidationException("有同学借鉴的计划，不能设置为私有");
                if (ParentId > 0)
                    throw new ValidationException("借鉴来的计划，不能设置为私有");
            }
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

        public void DeleteItem(long itemId, Guid operatorId = default(Guid))
        {
            var item = Items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                item.Delete(operatorId);
                DateUpdated = DateTimeOffset.Now;
                if (!item.ParentId.HasValue || item.ParentId < 1)
                {
                    AddDomainEvent(new ScheduleItemDeletedEvent(item.UUID));
                }
                else
                {
                    AddDomainEvent(new ChildScheduleItemDeletedEvent(item.UUID));
                }
            }
        }

        #region Status
        public Status Status { get; private set; }

        public virtual void Disable()
        {
            Deleted = true;
            Status = Status.Disable;
        }

        public virtual void Restore()
        {
            Deleted = false;
            Status = Status.Normal;
        }

        #endregion
    }

    public enum Stage
    {
        Starting,
        Started,
        Completed
    }

    [Flags]
    public enum Status
    {
        Normal = 0,
        Disable = 1
    }
}
