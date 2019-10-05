using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.BlogApi
{
    public class FakeBlogApiService : IBlogApiService
    {
        public Task<IEnumerable<SummaryLinkDto>> RecentPostLinks(int blogId, int page = 1, int size = 10)
        {
            var faker = new Faker<SummaryLinkDto>();
            faker.RuleFor(x => x.PostId, f => f.Random.Int(0, 100));
            faker.RuleFor(x => x.Title, f => f.Lorem.Text());
            faker.RuleFor(x => x.Link, f => f.Internet.Url());
            IEnumerable<SummaryLinkDto> result = faker.Generate(size);
            return Task.FromResult(result);
        }
    }
}
