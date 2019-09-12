using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.MarkdownApi
{
    public class MarkdownApiService : IMarkdownApiService
    {
        private readonly IMarkdownApi _api;

        public MarkdownApiService(IMarkdownApi api)
        {
            _api = api;
        }

        public Task<string> ToHtml(string input)
        {
            return _api.ToHtml(input);
        }
    }
}
