using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;
using Cnblogs.Common;
using Cnblogs.Domain.Abstract;
using Mapster;

namespace Cnblogs.Academy.ServiceAgent.BlogApi
{
    public class BlogApiService : IBlogApiService
    {
        private readonly IBlogApi _blogApi;

        public BlogApiService(IBlogApi blogApi)
        {
            _blogApi = blogApi;
        }

        public async Task<IEnumerable<SummaryLinkDto>> RecentPostLinks(int blogId, int page = 1, int size = 10)
        {
            var posts = await _blogApi.GetPostsByBlogId(blogId, page, size);
            return posts.Select(x => new SummaryLinkDto
            {
                PostId = x.Id,
                Title = x.Title,
                Link = x.Url
            });
        }
    }

    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
