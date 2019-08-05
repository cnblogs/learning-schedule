using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.Events;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Cnblogs.Domain.Abstract;
using DotNetCore.CAP;
using Enyim.Caching;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.ScheduleAppService
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _repository;
        private readonly IUCenterService _uCenterService;
        private readonly ICapPublisher _bus;
        private readonly IMarkdownApiService _markdownApi;
        private readonly IMemcachedClient _cache;

        public ScheduleService(IScheduleRepository repository,
            IUCenterService uCenterService,
            ICapPublisher bus,
            IMarkdownApiService markdownApi,
            IMemcachedClient cache)
        {
            _repository = repository;
            _uCenterService = uCenterService;
            _bus = bus;
            _markdownApi = markdownApi;
            _cache = cache;
        }

        private bool InRecent(DateTimeOffset? point = null, int offset = 12)
        {
            if (!point.HasValue) return false;
            return (DateTimeOffset.Now - point.Value).Hours < offset;
        }

        public async Task<BooleanResult> AddAsync(ScheduleInputModel im, UserDto user)
        {
            var schedule = new Schedule(im.Title, im.Description, user.UserId, im.IsPrivate);
            _repository.AddSchedule(schedule);
            using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
            {
                if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
                {
                    await _bus.PublishAsync("Schedule.Created", new ScheduleCreatedEvent
                    {
                        Id = schedule.Id,
                        Title = schedule.Title,
                        Description = schedule.Description,
                        UserId = user.UserId,
                        Link = $"{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}",
                        IsPrivate = schedule.IsPrivate
                    });
                    await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                    return BooleanResult.Succeed(schedule.Id.ToString());
                }
                else
                {
                    return BooleanResult.Fail("保存失败，请稍后重试");
                }
            }
        }

        public async Task<IEnumerable<ScheduleDto>> GetRecentScheduleAsync(Stage? stage, int page, int size)
        {
            var query = _repository.Schedules.Where(x => !x.IsPrivate);
            if (stage.HasValue)
            {
                query = query.Where(x => x.Stage == stage.Value);
            }
            var schedules = await query.OrderByDescending(x => x.DateUpdated)
                                    .Skip(--page * size).Take(size)
                                    .ProjectToType<ScheduleDto>()
                                    .ToArrayAsync();
            var users = await _uCenterService.GetUsersByUserIds(schedules.Select(x => x.UserId).ToArray());
            return schedules.Join(users, x => x.UserId, u => u.UserId, (x, u) => x.PatchUserInfo(u));
        }

        public async Task<long> AddItem(long id, ScheduleItemMarkdownInput inputModel, UserDto user)
        {
            var html = await _markdownApi.ToHtml(inputModel.Title);
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(x => x.Id == id);
            if (schedule == null) throw new ValidationException("无效的请求");

            var item = ScheduleItem.CreateMarkdownItem(id, inputModel.Title, user.UserId, html);
            schedule.AddItem(item);

            using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
            {
                if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
                {
                    await _bus.PublishAsync("ScheduleItem.Created", new ScheduleItemCreatedEvent
                    {
                        Id = item.Id,
                        ScheduleUserId = schedule.UserId,
                        UserId = user.UserId,
                        Title = item.GenerateDescription(),
                        Link = $"{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}/item/{item.Id}",
                        IsPrivate = schedule.IsPrivate
                    });
                    await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                    return item.Id;
                }
                else
                {
                    throw new Exception("保存失败，请稍后重试");
                }
            }
        }

        public async Task IncreaseCommentCountAsync(long objectId, int amount = 1)
        {
            var item = await _repository.Records.FirstOrDefaultAsync(i => i.Id == objectId);
            if (item != null)
            {
                item.CommentCount += amount;
                await _repository.UnitOfWork.SaveChangesAsync();
            }
        }

        public async Task IncreaseLikeCountAsync(long objectId, int amount = 1)
        {
            var item = await _repository.Records.FirstOrDefaultAsync(i => i.Id == objectId);
            if (item != null)
            {
                item.LikeCount += amount;
                await _repository.UnitOfWork.SaveChangesAsync();
            }
        }

        public async Task<BooleanResult> AddFollowingAsync(long id, Guid userId)
        {
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule != null)
            {
                try
                {
                    var result = schedule.AddFollowing(userId);
                    if (result.ok)
                        await _repository.UnitOfWork.SaveChangesAsync();
                    else
                        return BooleanResult.Fail(result.msg);
                }
                catch (Exception ex) when (ex.InnerException is SqlException sqlerror)
                {
                    if (sqlerror.ErrorCode == -2146232060)
                    {
                        return BooleanResult.Succeed("已经参加过了");
                    }
                }
            }

            return BooleanResult.Succeed();
        }

        public async Task UpdateScheduleAsync(long id, ScheduleInputModel input, Guid userId)
        {
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (schedule != null)
            {
                schedule.Update(input.Title, input.Description, input.IsPrivate);
                using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
                {
                    await _repository.UnitOfWork.SaveChangesAsync();
                    await _bus.PublishAsync("Schedule.Updated", new ScheduleUpdatedEvent
                    {
                        Id = id,
                        Title = schedule.Title,
                        Description = schedule.Description,
                        UserId = schedule.UserId
                    });
                }
                if (InRecent(schedule.DateAdded))
                {
                    await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                }
            }
        }

        public async Task DeleteScheduleAsync(long id, Guid userId)
        {
            var schedule = await _repository.Schedules.Include(x => x.Items)
                                    .ThenInclude(x => x.Records)
                                    .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (schedule != null)
            {
                schedule.Delete();

                using (var trans = await _repository.BeginTransactionAsync())
                {
                    if (await _repository.UnitOfWork.SaveChangesAsync() <= 0)
                    {
                        trans.Rollback();
                        return;
                    }
                    if (schedule.Items.Any())
                    {
                        var events = schedule.Items.Select(x => new ScheduleDeletedEvent
                        {
                            Id = x.Id,
                            UserId = x.UserId
                        });
                        await _bus.PublishAsync("ScheduleItems.Deleted", events);
                    }
                    await _bus.PublishAsync("Schedule.Deleted", new ScheduleDeletedEvent
                    {
                        Id = schedule.Id,
                        UserId = schedule.UserId,
                    });
                    trans.Commit();
                }

                if (InRecent(schedule.DateAdded) || InRecent(schedule.DateEnd))
                {
                    await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                    return;
                }
                if (schedule.Items.Any())
                {
                    if (InRecent(schedule.Items.Max(x => x.DateAdded)))
                    {
                        await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                        return;
                    }
                    var records = schedule.Items.SelectMany(x => x.Records);
                    if (records.Any())
                    {
                        if (InRecent(records.Max(x => x.DoneTime)))
                        {
                            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                            return;
                        }
                    }
                }
            }
        }

        public async Task<RecordItemDto> ItemDoneRecordOf(long id)
        {
            var item = await _repository.Records.Where(r => r.Id == id)
                .Select(r => new
                {
                    Id = r.ItemId,
                    ScheduleId = r.Item.ScheduleId,
                    Title = r.Item.Title,
                    UserId = r.UserId
                })
                .FirstOrDefaultAsync();
            if (item == null) return null;
            var user = await _uCenterService.GetUser(u => u.UserId, item.UserId);
            return new RecordItemDto
            {
                SpaceUserId = user.SpaceUserId,
                Title = item.Title,
                Name = user.DisplayName,
                Url = $"//academy.cnblogs.com/schedules/u/{user.Alias}/{item.ScheduleId}/detail/{item.Id}",
                UserId = user.UserId
            };
        }

        public async Task DeleteSchedulesByUserId(Guid userId)
        {
            var schedules = await _repository.Schedules.Where(s => s.UserId == userId).ToListAsync();
            foreach (var item in schedules)
            {
                item.Disable();
            }

            var followingSchedules = await _repository.ScheduleFollowing.Include(f => f.Schedule)
                .Where(f => f.UserId == userId).ToListAsync();
            foreach (var item in followingSchedules)
            {
                item.Disable();
            }

            var records = await _repository.Records.Include(r => r.Item).Where(r => r.UserId == userId).ToListAsync();
            foreach (var item in records)
            {
                item.Disable();
            }

            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task RestoreSchedulesByUserId(Guid userId)
        {
            var schedules = await _repository.Schedules.IgnoreQueryFilters()
                .Where(s => s.UserId == userId && s.Status.HasFlag(Status.Disable)).ToListAsync();
            foreach (var item in schedules)
            {
                item.Restore();
            }

            var followingSchedules = await _repository.ScheduleFollowing.IgnoreQueryFilters().Include(f => f.Schedule)
                .Where(f => f.UserId == userId && f.Status.HasFlag(Status.Disable)).ToListAsync();
            foreach (var item in followingSchedules)
            {
                item.Restore();
            }

            var records = await _repository.Records.IgnoreQueryFilters().Include(r => r.Item)
                .Where(r => r.UserId == userId && r.Status.HasFlag(Status.Disable)).ToListAsync();
            foreach (var item in records)
            {
                item.Restore();
            }

            await _repository.UnitOfWork.SaveChangesAsync();
        }

        public async Task<(IEnumerable<MyScheduleDto> list, int count)> GetMySchedules(Guid userId, bool completed, int pageIndex, int pageSize)
        {
            var query = _repository.Schedules.Where(s => s.UserId == userId && s.DateEnd.HasValue == completed);
            var count = await query.CountAsync();
            if (completed)
            {
                query = query.OrderByDescending(x => x.DateEnd);
            }
            else
            {
                query = query.OrderByDescending(x => x.DateAdded);
            }
            var list = await query.ProjectToType<MyScheduleDto>()
            .Skip(pageIndex * pageSize).Take(pageSize)
            .ToListAsync();
            return (list, count);
        }

        public async Task<BooleanResult> CompleteSchedule(long scheduleId, UserDto user, bool cancel = false)
        {
            var schedule = await _repository.Schedules.Where(s => s.UserId == user.UserId && s.Id == scheduleId)
            .FirstOrDefaultAsync();

            if (schedule == null) return BooleanResult.Fail("找不到对应的目标");

            if (!cancel)
            {
                schedule.MarkAsComplete();
            }
            else
            {
                schedule.CancelComplete();
            }
            using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
            {
                if (await _repository.UnitOfWork.SaveChangesAsync() <= 0)
                {
                    return BooleanResult.Fail("保存失败，请稍后重试");
                }
                if (!cancel)
                {
                    await _bus.PublishAsync("Schedule.Completed", new ScheduleCompletedEvent
                    {
                        Id = schedule.Id,
                        UserId = user.UserId,
                        Title = schedule.Title,
                        Link = $"{AppConst.DomainAddress}/schedules/u/{user.Alias}/{schedule.Id}",
                        IsPrivate = schedule.IsPrivate
                    });
                }
                else
                {
                    await _bus.PublishAsync("Schedule.Completed.Canceled", new ScheduleCompletedCanceledEvent
                    {
                        Id = schedule.Id,
                        UserId = schedule.UserId
                    });
                }
            }
            if (InRecent(schedule.DateAdded) || InRecent(schedule.DateEnd))
            {
                await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
            }
            return BooleanResult.Succeed();
        }

        public async Task UpdateItemTitleWithMarkdown(long id, string title, Guid userId)
        {
            var html = await _markdownApi.ToHtml(title);
            var item = await _repository.ScheduleItems.Include(i => i.Html).FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            if (item == null)
            {
                throw new ValidationException("非法的修改请求");
            }
            else
            {
                item.UpdateTitle(title, userId, TextType.Markdown, html);
                using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
                {
                    if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
                    {
                        await _bus.PublishAsync("ScheduleItem.Updated", new ScheduleItemUpdatedEvent
                        {
                            Id = item.Id,
                            Title = item.GenerateDescription(),
                            UserId = item.UserId
                        });
                    }
                }
                if (InRecent(item.DateAdded) || InRecent(item.DateEnd))
                {
                    await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                }
            }
        }

        public async Task DeleteItem(long id, Guid userId)
        {
            var item = await _repository.ScheduleItems.Include(x => x.Schedule).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (item != null)
            {
                item.Schedule.DeleteItems();
                using (_repository.UnitOfWork.Database.BeginTransaction(_bus, true))
                {
                    if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
                    {
                        await _bus.PublishAsync("ScheduleItem.Deleted", new ScheduleItemDeletedEvent
                        {
                            Id = item.Id,
                            UserId = item.UserId
                        });
                        if (InRecent(item.DateAdded) || InRecent(item.DateEnd))
                        {
                            await _cache.RemoveAsync(CacheKeyStore.HomeFeeds());
                        }
                    }
                }
            }
        }

        public async Task<PagedResults<ScheduleDetailDto>> ListWithItemsAsync(Guid userId, bool hasPrivate, bool completed,
            bool teachOnly, int page, int size)
        {
            IQueryable<Schedule> query;
            if (teachOnly)
            {
                hasPrivate = false;
                query = _repository.ScheduleItems.Where(x => x.UserId == userId).Select(x => x.Schedule)
                    .Where(x => x.UserId != userId).Distinct();
            }
            else
            {
                query = _repository.Schedules.Where(x => x.UserId == userId);
            }

            if (completed)
                query = query.Where(x => x.Stage == Stage.Completed);
            else
                query = query.Where(x => x.Stage != Stage.Completed);

            if (!hasPrivate)
                query = query.Where(x => x.IsPrivate == false);

            query = query.OrderByDescending(x => x.DateUpdated);

            var list = await query.Skip(--page * size).Take(size).ProjectToType<ScheduleDetailDto>().ToListAsync();
            var count = await query.CountAsync();
            if (list != null && list.Count > 0)
            {
                var users = await _uCenterService.GetUsersByUserIds(list.Select(x => x.UserId).Distinct().ToArray());
                list = list.Join(users, x => x.UserId, x => x.UserId, (s, u) =>
                 {
                     s.PatchUserInfo(u);
                     return s;
                 }).ToList();
            }
            return new PagedResults<ScheduleDetailDto>(count, list);
        }

        public async Task<ScheduleDetailDto> GetScheduleDetailAsync(long scheduleId, bool isOwner)
        {
            var query = _repository.Schedules.Where(x => x.Id == scheduleId);
            if (!isOwner)
            {
                query = query.Where(x => x.IsPrivate != true);
            }
            var schedule = await query.ProjectToType<ScheduleDetailDto>().FirstOrDefaultAsync();
            if (schedule == null) return null;

            var user = await _uCenterService.GetUser(x => x.UserId, schedule.UserId);
            schedule.PatchUserInfo(user);

            var users = await _uCenterService.GetUsersByUserIds(schedule.Items.Select(x => x.UserId).ToArray());
            schedule.Items = schedule.Items.Join(users, x => x.UserId, u => u.UserId, (x, u) =>
            {
                x.User = new AcademyUserDto(u);
                return x;
            });
            return schedule;
        }

        public async Task<DateTimeOffset?> LastPrivateUpdateTime(long scheduleId)
        {
            var record = await _repository.PrivateUpdateRecord.Where(x => x.ScheduleId == scheduleId).OrderByDescending(x => x.DateAdded).FirstOrDefaultAsync();
            return record?.DateAdded;
        }

        public async Task UpdatePrivateAsync(long scheduleId, bool to, Guid userId)
        {
            var lastUpdateTime = await LastPrivateUpdateTime(scheduleId);
            if (lastUpdateTime.HasValue)
            {
                if (lastUpdateTime.Value > DateTimeOffset.Now.AddHours(-24))
                {
                    return;
                }
            }
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(x => x.Id == scheduleId && x.UserId == userId);
            if (schedule != null)
            {
                schedule.TogglePrivate(to);

                var itemIds = await _repository.ScheduleItems.Where(x => x.ScheduleId == scheduleId).Select(x => x.Id).ToArrayAsync();

                using (_repository.UnitOfWork.Database.BeginTransaction(_bus, autoCommit: true))
                {
                    if (await _repository.UnitOfWork.SaveChangesAsync() > 0)
                    {
                        await _bus.PublishAsync("SchedulePrivate.Updated", new SchedulePrivateUpdatedEvent
                        {
                            Id = schedule.Id,
                            UserId = schedule.UserId,
                            IsPrivate = schedule.IsPrivate,
                            ItemIds = itemIds
                        });
                    }
                }
            }
        }
    }
}
