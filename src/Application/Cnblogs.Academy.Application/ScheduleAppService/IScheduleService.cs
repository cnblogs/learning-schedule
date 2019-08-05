using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public interface IScheduleService
    {
        Task<BooleanResult> AddAsync(ScheduleInputModel input, UserDto user);
        Task<IEnumerable<ScheduleDto>> GetRecentScheduleAsync(Stage? stage, int page, int size);
        Task<long> AddItem(long id, ScheduleItemMarkdownInput input, UserDto user);
        Task IncreaseCommentCountAsync(long objectId, int amount = 1);
        Task IncreaseLikeCountAsync(long objectId, int amount = 1);
        Task<BooleanResult> AddFollowingAsync(long id, Guid userId);
        Task UpdateScheduleAsync(long id, ScheduleInputModel input, Guid userId);
        Task DeleteScheduleAsync(long id, Guid userId);
        Task<RecordItemDto> ItemDoneRecordOf(long id);
        Task DeleteSchedulesByUserId(Guid userId);
        Task RestoreSchedulesByUserId(Guid userId);
        Task<(IEnumerable<MyScheduleDto> list, int count)> GetMySchedules(Guid userId, bool completed, int pageIndex, int pageSize);
        Task<BooleanResult> CompleteSchedule(long scheduleId, UserDto user, bool cancel = false);
        Task UpdateItemTitleWithMarkdown(long id, string title, Guid userId);
        Task DeleteItem(long id, Guid userId);
        Task<PagedResults<ScheduleDetailDto>> ListWithItemsAsync(Guid userId, bool hasPrivate, bool completed,
            bool teachOnly, int page, int size);
        Task<ScheduleDetailDto> GetScheduleDetailAsync(long scheduleId, bool isOwner);
        Task<DateTimeOffset?> LastPrivateUpdateTime(long scheduleId);
        Task UpdatePrivateAsync(long scheduleId, bool to, Guid userId);
    }
}
