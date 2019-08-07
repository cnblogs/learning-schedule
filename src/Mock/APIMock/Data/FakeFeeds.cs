using System.Collections.Generic;
using Bogus;
using Cnblogs.Academy.DTO;

namespace APIMock.Data
{
    public static class FakeFeeds
    {
        public static IEnumerable<FeedDto> Feeds()
        {
            var faker = new Faker<FeedDto>()
                .RuleFor(x => x.Alias, f => f.Name.FirstName())
                .RuleFor(x => x.Link, f => f.Internet.Url())
                .RuleFor(x => x.UserName, f => f.Internet.UserName());
            faker.RuleFor(x => x.DateAdded, dateFaker => dateFaker.Date.Recent(7));
            faker.RuleFor(x => x.FeedTitle, strFaker => strFaker.Lorem.Sentence());
            return faker.GenerateLazy(500);
        }
    }
}
