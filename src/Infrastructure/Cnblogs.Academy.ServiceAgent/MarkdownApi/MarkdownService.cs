using System.Threading.Tasks;
using Markdig;

namespace Cnblogs.Academy.ServiceAgent.MarkdownApi
{
    public class MarkdownService : IMarkdownApiService
    {
        private readonly AutoLinkTitleService _autoLinkTitleSvc;

        public MarkdownService(AutoLinkTitleService svc)
        {
            _autoLinkTitleSvc = svc;
        }

        public async Task<string> ToHtml(string input)
        {
            var pipeline = new MarkdownPipelineBuilder().DisableHtml().UseAutoLinks().Build();
            var html = Markdown.ToHtml(input, pipeline);
            return await _autoLinkTitleSvc.ReRender(html);
        }
    }
}
