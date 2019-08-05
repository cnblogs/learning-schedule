using System;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Queries
{
    public class ScheduleQueries : IScheduleQueries
    {
        private readonly IScheduleRepository _repository;
        private readonly IUCenterService _uCenterSvc;

        public ScheduleQueries(IScheduleRepository repository, IUCenterService uCenterSvc)
        {
            _uCenterSvc = uCenterSvc;
            _repository = repository;
        }

        public async Task<ScheduleItemDetailDto> GetScheduleItemDetailAsync(long itemId, Guid userId)
        {
            var detail = await _repository.ScheduleItems.Include(x => x.Html).Include(x => x.Subtasks)
            .Include(x => x.References).Where(x => x.Id == itemId)
            .ProjectToType<ScheduleItemDetailDto>().FirstOrDefaultAsync();
            if (detail == null) return null;
            var user = await _uCenterSvc.GetUser(x => x.UserId, detail.UserId);
            detail.User = new AcademyUserDto(user);
            return detail;
        }
    }
}
