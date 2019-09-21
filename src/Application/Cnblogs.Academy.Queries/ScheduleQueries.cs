using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Cache;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Enyim.Caching;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Queries
{
    public class ScheduleQueries : IScheduleQueries
    {
        private readonly IScheduleRepository _repository;
        private readonly IUCenterService _uCenterSvc;
        private readonly IMemcachedClient _cache;

        public ScheduleQueries(IScheduleRepository repository, IUCenterService uCenterSvc, IMemcachedClient cache)
        {
            _uCenterSvc = uCenterSvc;
            _cache = cache;
            _repository = repository;
        }

        public async Task<ScheduleItemDetailDto> GetScheduleItemDetailAsync(long itemId, Guid userId)
        {
            var detail = await _repository.ScheduleItems.Include(x => x.Html)
                                                        .Where(x => x.Id == itemId)
                                                        .ProjectToType<ScheduleItemDetailDto>()
                                                        .FirstOrDefaultAsync();
            if (detail == null) return null;
            var user = await _uCenterSvc.GetUser(x => x.UserId, detail.UserId);
            detail.User = new AcademyUserDto(user);
            return detail;
        }

        public async Task<SummaryDto> GetSummary(long itemId)
        {
            return await _repository.ScheduleItems
                                    .Where(x => x.Id == itemId)
                                    .ProjectToType<SummaryDto>()
                                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ScheduleFollowingDto>> GetScheduleFollowings(long id)
        {
            return await _cache.GetValueOrCreateAsync(CacheKeyStore.ScheduleFollowings(id),
                                                      3600 * 24,
                                                      () => GetCacheScheduleFollowings(id));
        }
        private async Task<IEnumerable<ScheduleFollowingDto>> GetCacheScheduleFollowings(long id)
        {
            var followings = await _repository.Schedules
                                              .Where(x => x.ParentId == id)
                                              .Select(x => new ScheduleFollowingDto
                                              {
                                                  ScheduleId = x.Id,
                                                  UserId = x.UserId
                                              }).ToListAsync();

            var users = await _uCenterSvc.GetUsersByUserIds(followings.Select(x => x.UserId).ToArray());
            followings = followings.Join(users, x => x.UserId, u => u.UserId, (x, user) =>
             {
                 x.User = new AcademyUserDto(user);
                 return x;
             }).ToList();
            return followings;
        }

        public async Task<List<KeyValuePair<long, string>>> GetScheduleOptions(Guid userId, int page, int size)
        {
            return await _repository.Schedules.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateUpdated)
                .Select(x => new KeyValuePair<long, string>(x.Id, x.Title))
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
        }
    }
}
