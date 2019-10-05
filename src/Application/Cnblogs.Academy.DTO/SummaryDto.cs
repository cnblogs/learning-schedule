using System.Collections.Generic;

namespace Cnblogs.Academy.DTO
{
    public class SummaryDto
    {
        public long ItemId { get; set; }
        public SummaryNoteDto Note { get; set; }
        public IEnumerable<SummaryLinkDto> Links { get; set; }
    }

    public class SummaryNoteDto
    {
        public long Id { get; set; }
        public string Note { get; set; }
        public string Html { get; set; }
    }
    
    public class SummaryLinkDto
    {
        public long Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public int PostId { get; set; }
    }
}
