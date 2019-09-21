using System;
using System.ComponentModel.DataAnnotations;
using Cnblogs.Academy.Domain;

namespace Cnblogs.Academy.DTO
{
    public class FeedInputModel
    {
        public string ContentId { get; set; }
        [MaxLength(200)]
        public string FeedTitle { get; set; }
        [MaxLength(300)]
        public string FeedContent { get; set; }
        [MaxLength(200)]
        [Required]
        public string Link { get; set; }
        public DateTime? DateAdded { get; set; }
        public Guid UserId { get; set; }
        public Guid AppId { get; set; }
        public Guid ApplicationId { get; set; }
        public FeedType FeedType { get; set; }
        public bool IsPrivate { get; set; }
    }
}