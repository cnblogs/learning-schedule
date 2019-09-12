using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.MarkdownApi
{
    public interface IMarkdownApi
    {
        [Post("/markdown/tohtml")]
        Task<string> ToHtml([Body]string input);
    }
}
