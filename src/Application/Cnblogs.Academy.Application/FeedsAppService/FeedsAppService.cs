using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain;
using Cnblogs.Common;
using Cnblogs.Feed.DTO;
using Cnblogs.Feed.ServiceAgent;
using Cnblogs.UCenter.ServiceAgent;
using DotNetCore.CAP;
using Enyim.Caching;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public class FeedsAppService : IFeedsAppService, ICapSubscribe
    {
        private readonly IFeedServiceAgent _feedSvc;
        private readonly IUCenterService _uCenter;
        private readonly IMemcachedClient _cache;

        public FeedsAppService(
            IFeedServiceAgent feedSvc,
            IUCenterService uCenter,
            IMemcachedClient cache)
        {
            _uCenter = uCenter;
            _feedSvc = feedSvc;
            _cache = cache;
        }

        public async Task<IEnumerable<FeedDto>> GetAcademyFeedsAsync(int page, int size)
        {
            return await _cache.GetValueOrCreateAsync<IEnumerable<FeedDto>>(CacheKeyStore.HomeFeeds(page), 5 * 60,
                () => _feedSvc.GetFeedsByAppIdAsync(AppConst.AppId, page, size));
        }

        public async Task<PagedResult<FeedDto>> GetConcernFeeds(int page, int size, Guid userId, Guid? groupId = null)
        {
            return await _cache.GetValueOrCreateAsync<PagedResult<FeedDto>>(CacheKeyStore.IndexFeeds(page, userId, groupId), 5 * 60,
            () => _feedSvc.FetchConcernAsync(AppConst.AppGuid, userId, page, size, groupId));
        }

        public async Task<PagedResult<FeedDto>> GetFeeds(string alias, int pageIndex, int pageSize, bool withPrivate = false)
        {
            var user = await _uCenter.GetUser(x => x.Alias, alias);
            if (user == null) return PagedResult<FeedDto>.Empty();

            return await _feedSvc.GetMyFeedsAsync(AppConst.AppId, user.UserId, pageIndex, pageSize, withPrivate);
        }
    }
}
