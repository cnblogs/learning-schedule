using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;
using Cnblogs.Domain.Abstract;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.FeedsApi
{
    public interface IFeedsApi
    {
        [Get("/api/feed/")]
        Task<IEnumerable<FeedDto>> GetFeedsByAppId(string appId, int page = 1, int size = 30);

        [Get("/api/feed/{userId}")]
        Task<PagedResults<FeedDto>> GetMyFeeds(string appId, Guid userId, int page, int size, bool withPrivate = false);

        [Get("/api/feed/concern")]
        Task<PagedResults<FeedDto>> GetConcernFeeds(string appId, int page, int size, Guid userId, Guid? groupId);
    }
}
