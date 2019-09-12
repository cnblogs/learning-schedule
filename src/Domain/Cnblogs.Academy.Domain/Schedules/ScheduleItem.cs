using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cnblogs.Academy.Domain.Schedules.Events;
using NUglify;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class ScheduleItem : Entity
    {
        private ScheduleItem()
        {
        }

        public ScheduleItem(long scheduleId, string title, Guid userId, DateTimeOffset? dateStart = null) : this()
        {
            ScheduleId = scheduleId;
            Title = title;
            UserId = userId;
            if (dateStart.HasValue && dateStart != default(DateTimeOffset))
            {
                DateStart = dateStart.Value;
            }
            AddDomainEvent(new ScheduleItemCreatedEvent(UUID));
        }

        public long ScheduleId { get; private set; }

        public Schedule Schedule { get; private set; }

        public string Title { get; private set; }

        /// <summary>
        /// 主要是用来排序
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset DateStart { get; private set; }

        public DateTimeOffset? DateEnd { get; private set; }
        public bool Completed
        {
            get
            {
                return DateEnd.HasValue && DateEnd != default(DateTimeOffset);
            }
        }
        public Guid UserId { get; private set; }


        [Timestamp]
        public byte[] Timestamp { get; private set; }

        public TextType TextType { get; private set; }

        public ScheduleItemHtml Html { get; private set; }

        public SummaryNote SummaryNote { get; private set; }
        public ICollection<SummaryLink> SummaryLinks { get; private set; }

        public long? ParentId { get; private set; }
        public ScheduleItem Parent { get; private set; }
        public ICollection<ScheduleItem> Children { get; private set; }

        public void Complete(Guid userId, DateTimeOffset? doneTime)
        {
            if (UserId == userId)
            {
                DateStart = doneTime ?? DateTimeOffset.Now;
            }
            DateEnd = DateTimeOffset.Now;
        }

        public void UpdateTitle(string title, TextType type = TextType.PlainText, string html = "", Guid operateorId = default(Guid))
        {
            if (ParentId > 0)
            {
                if (operateorId == UserId)
                    throw new ValidationException("很抱歉，不能修改借鉴来的学习任务");
            }

            var legacyTitle = GenerateDescription();
            Title = title;
            TextType = type;
            if (type == TextType.Markdown)
            {
                Html = Html ?? new ScheduleItemHtml();
                Html.Html = html;
            }
            if (!ParentId.HasValue || ParentId < 1)
            {
                AddDomainEvent(new ScheduleItemUpdatedEvent(UUID, legacyTitle));
            }
            else
            {
                AddDomainEvent(new ChildScheduleItemUpdatedEvent(UUID, legacyTitle));
            }
        }

        public void Delete(Guid operateorId = default(Guid))
        {
            if (ParentId > 0)
            {
                if (operateorId == UserId)
                    throw new ValidationException("很抱歉，不能删除借鉴来的学习任务");
            }
            Deleted = true;
        }

        public static ScheduleItem CreateMarkdownItem(long scheduleId, string markdown, Guid userId, string html)
        {
            var item = new ScheduleItem(scheduleId, markdown, userId)
            {
                TextType = TextType.Markdown,
                Html = new ScheduleItemHtml
                {
                    Html = html
                }
            };
            return item;
        }

        public string GenerateDescription()
        {
            string description;
            if (TextType != TextType.PlainText)
            {
                description = Uglify.HtmlToText(Html.Html).Code;
            }
            else
            {
                description = Title;
            }
            return description?.Length > 150 ? description?.Substring(0, 148) + "..." : description;
        }

        public void Undo()
        {
            DateEnd = null;
            AddDomainEvent(new ScheduleItemUndoEvent(Id, UserId));
        }

        public SummaryNote AddSummaryNote(long itemId, string value, string html, Guid userId)
        {
            if (UserId == userId)
            {
                SummaryNote = new SummaryNote(itemId, value, html, userId);
                return SummaryNote;
            }
            else
            {
                return null;
            }
        }

        public void UpdateSummaryNote(string note, string html, Guid userId)
        {
            if (UserId == userId)
            {
                SummaryNote.Update(note, html);
            }
        }

        public SummaryLink AddSummaryLink(Guid userId, int postId, string title, string link)
        {
            SummaryLinks = SummaryLinks ?? new List<SummaryLink>();
            if (SummaryLinks.Count >= 10)
            {
                throw new ValidationException("最多关联10篇博文");
            }
            var existLink = SummaryLinks.FirstOrDefault(x => x.PostId == postId);
            if (existLink != null)
            {
                existLink.Update(title, link);
                return existLink;
            }
            else
            {
                var summaryLink = new SummaryLink(userId, postId, title, link);
                SummaryLinks.Add(summaryLink);
                return summaryLink;
            }
        }

        public void RemoveSummaryLink(Guid userId, long linkId)
        {
            var link = SummaryLinks.FirstOrDefault(x => x.UserId == userId && x.Id == linkId);
            link.Delete();
        }

        public ScheduleItem Deliver(long scheduleId, Guid userId)
        {
            ScheduleItem child;
            if (TextType == TextType.Markdown)
            {
                child = CreateMarkdownItem(scheduleId, Title, userId, Html.Html);
            }
            else
            {
                child = new ScheduleItem(scheduleId, Title, userId);
            }
            child.ParentId = Id;
            return child;
        }

        public ScheduleItem Dispatch(long scheduleId, Guid userId)
        {
            var child = Deliver(scheduleId, userId);
            AddDomainEvent(new ScheduleItemDispatchedEvent(child.UUID));
            return child;
        }
    }

    public class ScheduleItemHtml
    {
        public long Id { get; private set; }
        public long ScheduleItemId { get; private set; }
        public ScheduleItem Item { get; private set; }
        public string Html { get; internal set; }
    }
}
