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
        Task<PagedResult<FeedDto>> GetMyFeeds(string appId, Guid userId, int page, int size, bool withPrivate = false);

        [Get("/api/feed/concern")]
        Task<PagedResult<FeedDto>> GetConcernFeeds(string appId, int page, int size, Guid userId, Guid? groupId);
        
        [Delete("/api/feed")]
        Task Delete([Body]FeedDeletedInput model);

        [Post("/api/feed")]
        Task Publish([Body]FeedInputModel feedInputModel);

        [Put("/api/feed")]
        Task Update([Body]FeedUpdateModel model);
    }
}
