using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUglify;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class ScheduleItem
    {
        public ScheduleItem()
        {
            Records = new List<ItemDoneRecord>();
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
        }

        public long Id { get; set; }
        public long ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateAdded { get; set; }

        /// <summary>
        /// 主要是用来排序
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset DateStart { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        public bool Completed
        {
            get
            {
                return DateEnd.HasValue && DateEnd != default(DateTimeOffset);
            }
        }
        public Guid UserId { get; set; }
        public int DoneCount { get; set; }
        public ICollection<ItemDoneRecord> Records { get; set; }
        public bool Deleted { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public TextType TextType { get; set; }

        public ScheduleItemHtml Html { get; set; }

        public ICollection<Subtask> Subtasks { get; private set; }
        public ICollection<Reference> References { get; private set; }
        public ICollection<Feedback> Feedbacks { get; private set; }

        public ItemDoneRecord Done(Guid userId, string content, DateTimeOffset? doneTime)
        {
            if (UserId == userId)
            {
                DateStart = doneTime.HasValue ? doneTime.Value : DateTimeOffset.Now;
            }
            DoneCount += 1;
            DateEnd = DateTimeOffset.Now;
            var record = new ItemDoneRecord(Id, userId, content, doneTime);
            Records.Add(record);
            return record;
        }

        public void UpdateTitle(string title, Guid userId, TextType type = TextType.PlainText, string html = "")
        {
            if (UserId == userId)
            {
                Title = title;
                TextType = type;
                if (type == TextType.Markdown)
                {
                    Html = Html ?? new ScheduleItemHtml();
                    Html.Html = html;
                }
            }
        }

        public void Delete()
        {
            Deleted = true;
        }

        public static ScheduleItem CreateMarkdownItem(long scheduleId, string markdown, Guid userId, string html)
        {
            var item = new ScheduleItem(scheduleId, markdown, userId);
            item.TextType = TextType.Markdown;
            item.Html = new ScheduleItemHtml
            {
                Html = html
            };
            return item;
        }

        public string GenerateDescription()
        {
            var description = "";
            if (!string.IsNullOrEmpty(Html.Html))
            {
                description = Uglify.HtmlToText(Html.Html).Code;
            }
            else
            {
                description = Title;
            }
            return description?.Length > 150 ? description?.Substring(0, 148) + "..." : description;
        }

        public void Undo(Guid userId)
        {
            DateEnd = null;

            if (Records != null && Records.Any())
            {
                foreach (var record in Records)
                {
                    if (record.UserId == userId)
                    {
                        record.Delete();
                    }
                }
            }
        }


        public void AddReference(Reference reference)
        {
            References = References ?? new List<Reference>();
            References.Add(reference);
        }

        public void AddSubtask(Subtask subtask)
        {
            Subtasks = Subtasks ?? new List<Subtask>();
            Subtasks.Add(subtask);
        }

        public Feedback AddFeedback(Difficulty difficulty, string content, Guid userId)
        {
            Feedbacks = new List<Feedback>();
            var feedback = new Feedback(Id, difficulty, content, userId);
            Feedbacks.Add(feedback);
            return feedback;
        }
    }

    public class ScheduleItemHtml
    {
        public long Id { get; set; }
        public long ScheduleItemId { get; set; }
        public ScheduleItem Item { get; set; }
        public string Html { get; set; }
    }
}
