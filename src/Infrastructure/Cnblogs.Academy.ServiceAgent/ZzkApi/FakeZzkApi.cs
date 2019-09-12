using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Cnblogs.Academy.DTO.Books;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.ZzkApi
{
    public class FakeZzkApi : IZzkApi
    {
        public Task<ZzkResponse> Search([Body] QueryModel model)
        {
            var faker = new Faker<ZzkResponse>();
            faker.RuleFor(x => x.TotalCount, f => model.Page.Index * 10);

            var dockFaker = new Faker<Doc>();
            dockFaker.RuleFor(x => x.Document, f => new PostDto
            {
                Title = f.Lorem.Text(),
                Uri = $"http://www.cnblogs.com/{f.Random.Word()}/p/{f.Random.Int()}"
            });

            faker.RuleFor(x => x.Docs, f => dockFaker.Generate(model.Page.Size));

            return Task.FromResult(faker.Generate(1).FirstOrDefault());
        }
    }
}
