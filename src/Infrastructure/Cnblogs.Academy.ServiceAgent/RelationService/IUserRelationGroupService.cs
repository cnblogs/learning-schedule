using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enyim.Caching;

namespace Cnblogs.Academy.ServiceAgent.RelationService
{
    public interface IUserRelationGroupService
    {
        Task<IEnumerable<RelationGroupDto>> GetMyGroupsAsync(Guid userId);
    }

    public class UserRelationGroupService : IUserRelationGroupService
    {
        private IUserRelationGroupApi _api;
        private IMemcachedClient _cache;

        public UserRelationGroupService(IUserRelationGroupApi api, IMemcachedClient cache)
        {
            _api = api;
            _cache = cache;
        }

        public Task<IEnumerable<RelationGroupDto>> GetMyGroupsAsync(Guid userId)
        {
            var key = "ucenter_relation_groups_" + userId;
            return _cache.GetValueOrCreateAsync(key, 3600 * 10, () => _api.GetMyGroups(userId));
        }
    }
}
