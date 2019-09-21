using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.BlogApi
{
    public interface IBlogApiService
    {
        Task<IEnumerable<SummaryLinkDto>> RecentPostLinks(int blogId, int page = 1, int size = 10);
    }
}
