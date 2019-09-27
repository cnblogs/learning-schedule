using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.FeedsApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Cnblogs.Domain.Abstract;
using Enyim.Caching;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public class FeedsAppService : IFeedsAppService
    {
        private readonly IFeedsApi _feedsApi;
        private readonly IUCenterService _uCenterSvc;
        private readonly IMemcachedClient _cache;

        public FeedsAppService(IFeedsApi feedsApi, IUCenterService uCenterSvc, IMemcachedClient cache)
        {
            _uCenterSvc = uCenterSvc;
            _feedsApi = feedsApi;
            _cache = cache;
        }

        public async Task<IEnumerable<FeedDto>> GetAcademyFeedsAsync(int page, int size)
        {
            return await _cache.GetValueOrCreateAsync<IEnumerable<FeedDto>>(CacheKeyStore.HomeFeeds(page), 5 * 60,
                () => _feedsApi.GetFeedsByAppId(AppConst.AppId, page, size));
        }

        public async Task<PagedResult<FeedDto>> GetConcernFeeds(int page, int size, Guid userId, Guid? groupId = null)
        {
            return await _cache.GetValueOrCreateAsync<PagedResult<FeedDto>>(CacheKeyStore.IndexFeeds(page, userId, groupId), 5 * 60,
            () => _feedsApi.GetConcernFeeds(AppConst.AppId, page, size, userId, groupId));
        }

        public async Task<PagedResult<FeedDto>> GetFeeds(string alias, int pageIndex, int pageSize, bool withPrivate = false)
        {
            var user = await _uCenterSvc.GetUser(x => x.Alias, alias);
            if (user == null) return PagedResult<FeedDto>.Empty();

            return await _feedsApi.GetMyFeeds(AppConst.AppId, user.UserId, pageIndex, pageSize, withPrivate);
        }
    }
}
