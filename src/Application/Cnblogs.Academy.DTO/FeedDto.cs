using System;
using Cnblogs.Academy.Domain;

namespace Cnblogs.Academy.DTO
{
    public class FeedDto
    {
        public string FeedTitle { get; set; }
        public FeedType FeedType { get; set; }
        public string UserName { get; set; }
        public DateTime DateAdded { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string Alias { get; set; }
        public bool IsPrivate { get; set; }
    }
}
