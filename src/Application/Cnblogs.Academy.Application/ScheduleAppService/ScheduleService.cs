using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.Events;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using Cnblogs.Domain.Abstract;
using Cnblogs.Academy.ServiceAgent.UCenterService;
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

        public ScheduleService(
            IScheduleRepository repository,
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

        public bool ShouldClearCache(DateTimeOffset? point = null, int offset = 12)
        {
            if (!point.HasValue) return false;
            return (DateTimeOffset.Now - point.Value).Hours < offset;
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

        public async Task<long> SubscribeAsync(long id, Guid userId)
        {
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(s => s.Id == id);
            if (schedule != null)
            {
                try
                {
                    var child = schedule.Subscribe(userId);
                    if (await _repository.UnitOfWork.SaveEntitiesAsync())
                    {
                        int page = 0, size = 10;
                        do
                        {
                            var items = await _repository.ScheduleItems.Include(x => x.Html)
                                .Where(x => x.ScheduleId == id)
                                .OrderBy(x => x.Id)
                                .Skip(page * size)
                                .Take(size)
                                .ToListAsync();

                            child.AcceptItemsFrom(items);
                            await _repository.UnitOfWork.SaveEntitiesAsync();

                            page++;
                            size = items.Count;
                        } while (size >= 10);
                        return child.Id;
                    }
                }
                catch (Exception ex) when (ex.InnerException is SqlException sqlerror)
                {
                    if (sqlerror.Number == 2601)
                    {
                        throw new ValidationException("已经借鉴过了");
                    }
                }
            }
            return 0;
        }

        public async Task DeleteSchedulesByUserId(Guid userId)
        {
            var schedules = await _repository.Schedules.Where(s => s.UserId == userId).ToListAsync();
            foreach (var schedule in schedules)
            {
                schedule.Disable();
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

        public async Task<PagedResult<ScheduleDetailDto>> ListWithItemsAsync(Guid userId, bool hasPrivate, bool completed,
            int page, int size)
        {
            IQueryable<Schedule> query = _repository.Schedules.Where(x => x.UserId == userId);

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
            return new PagedResult<ScheduleDetailDto>(count, list);
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

            if (schedule.Parent != null)
            {
                var pUser = await _uCenterService.GetUser(x => x.UserId, schedule.Parent.UserId);
                schedule.Parent.Alias = pUser.Alias;
                schedule.Parent.UserName = pUser.DisplayName;
            }
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
