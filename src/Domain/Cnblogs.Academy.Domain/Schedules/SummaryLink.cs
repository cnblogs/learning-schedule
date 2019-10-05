using System;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.Domain.Schedules
{
    public class SummaryLink : Entity
    {
        private SummaryLink()
        {
        }

        public SummaryLink(Guid userId, int postId, string title, string link)
        {
            UserId = userId;
            PostId = postId;
            Title = title;
            Link = link;
        }

        [MaxLength(500)]
        public string Title { get; private set; }

        [MaxLength(500)]
        public string Link { get; private set; }

        [Required]
        public int PostId { get; private set; }

        public Guid UserId { get; private set; }

        public long ItemId { get; private set; }

        public ScheduleItem Item { get; private set; }

        public void Update(string title, string link)
        {
            Title = title;
            Link = link;
        }

        internal void Delete()
        {
            Deleted = true;
        }
    }
}