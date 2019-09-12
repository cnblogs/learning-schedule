using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetRecentScheduleAsync(Stage? stage, int page, int size);
        Task<long> SubscribeAsync(long id, Guid userId);
        Task DeleteSchedulesByUserId(Guid userId);
        Task RestoreSchedulesByUserId(Guid userId);
        Task<(IEnumerable<MyScheduleDto> list, int count)> GetMySchedules(Guid userId, bool completed, int pageIndex, int pageSize);
        Task<PagedResults<ScheduleDetailDto>> ListWithItemsAsync(Guid userId, bool hasPrivate, bool completed,
            int page, int size);
        Task<ScheduleDetailDto> GetScheduleDetailAsync(long scheduleId, bool isOwner);
        Task<DateTimeOffset?> LastPrivateUpdateTime(long scheduleId);
        Task UpdatePrivateAsync(long scheduleId, bool to, Guid userId);
        bool ShouldClearCache(DateTimeOffset? point = null, int offset = 12);
    }
}
