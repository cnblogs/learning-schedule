using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.BlogApi
{
    public interface IBlogApi
    {
        [Get("/blogposts/blogId/{blogId}")]
        Task<IEnumerable<PostDto>> GetPostsByBlogId(int blogId, int? pageIndex = 1, int? pageSize = 10);
    }
}
