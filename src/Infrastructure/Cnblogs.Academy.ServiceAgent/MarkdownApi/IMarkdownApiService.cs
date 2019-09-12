using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.MarkdownApi
{
    public interface IMarkdownApiService
    {
        Task<string> ToHtml(string input);
    }
}
