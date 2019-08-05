using System;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.Application.Queries
{
    public interface IScheduleQueries
    {
        Task<ScheduleItemDetailDto> GetScheduleItemDetailAsync(long itemId, Guid userId);
    }
}
