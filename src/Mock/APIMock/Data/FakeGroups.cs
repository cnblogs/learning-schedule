using System;
using System.Collections.Generic;
using Bogus;
using Cnblogs.Academy.ServiceAgent.RelationService;

namespace APIMock.Data
{
    public static class FakeGroups
    {
        public static IEnumerable<RelationGroupDto> Groups(Guid userId)
        {
            var faker = new Faker<RelationGroupDto>();
            faker.RuleFor(x => x.UserId, userId);
            return faker.GenerateLazy(3);
        }
    }
}
