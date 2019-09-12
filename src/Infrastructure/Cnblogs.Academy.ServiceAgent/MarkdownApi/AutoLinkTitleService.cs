using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using NUglify;

namespace Cnblogs.Academy.ServiceAgent.MarkdownApi
{
    public class AutoLinkTitleService
    {
        public AutoLinkTitleService()
        {
        }

        public async Task<string> ReRender(string html)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));
            var links = document.Links.OfType<IHtmlAnchorElement>();
            foreach (var link in links)
            {
                if (link.Text.StartsWith("http"))
                {
                    var cts = new CancellationTokenSource();
                    var token = cts.Token;
                    cts.CancelAfter(2000);
                    var target = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(link.Href, token);
                    string title = RetrieveTitle(target);
                    title = CleanCrunchedHtml(title);
                    title = RetrieveHeading(target, title);
                    title = CleanCrunchedHtml(title);
                    if (!String.IsNullOrWhiteSpace(title))
                    {
                        if (title.Length > 200)
                        {
                            title = title.Substring(0, 200);
                        }
                        link.InnerHtml = title;
                    }
                }
            }
            return document.ToHtml();
        }

        private string CleanCrunchedHtml(string title)
        {
            if (!String.IsNullOrEmpty(title))
            {
                var result = Uglify.Html(title);
                if (!result.HasErrors)
                {
                    title = result.Code?.Trim();
                }
            }
            return title;
        }

        private string RetrieveHeading(AngleSharp.Dom.IDocument target, string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                title = target.Body.QuerySelectorAll("h1, h2, h3").FirstOrDefault()?.TextContent;
            }
            return title;
        }

        private string RetrieveTitle(AngleSharp.Dom.IDocument target)
        {
            return (target.Head.QuerySelector("title") as IHtmlTitleElement)?.Text;
        }
    }
}
