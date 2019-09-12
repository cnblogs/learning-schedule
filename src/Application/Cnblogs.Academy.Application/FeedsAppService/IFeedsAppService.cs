using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Common;
using Cnblogs.Feed.DTO;

namespace Cnblogs.Academy.Application.FeedsAppService
{
    public interface IFeedsAppService
    {
        Task<PagedResult<FeedDto>> GetFeeds(string alias, int page, int size, bool withPrivate = false);
        Task<IEnumerable<FeedDto>> GetAcademyFeedsAsync(int page, int size);
        Task<PagedResult<FeedDto>> GetConcernFeeds(int page, int size, Guid userId, Guid? groupId = null);
    }
}
