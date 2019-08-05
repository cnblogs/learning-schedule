using System;
using Cnblogs.Academy.Domain;
using Newtonsoft.Json;

namespace Cnblogs.Academy.DTO
{
    public class ScheduleItemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public TextType TextType { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public DateTimeOffset? DateEnd { get; set; }
        private string _html;
        public string Html
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_html))
                {
                    _html = Title;
                }
                return _html;
            }
            set
            {
                _html = value;
            }
        }

        [JsonIgnore]
        public Guid UserId { get; set; }
        public AcademyUserDto User { get; set; }
    }
}
