using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.Application.Queries
{
    public interface IScheduleQueries
    {
        Task<ScheduleItemDetailDto> GetScheduleItemDetailAsync(long itemId, Guid userId);
        Task<SummaryDto> GetSummary(long itemId);
        Task<IEnumerable<ScheduleFollowingDto>> GetScheduleFollowings(long id);
        Task<List<KeyValuePair<long, string>>> GetScheduleOptions(Guid userId, int page, int size);
    }
}
