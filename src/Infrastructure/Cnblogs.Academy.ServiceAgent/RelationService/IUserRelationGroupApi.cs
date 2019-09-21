using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.RelationService
{
    public interface IUserRelationGroupApi
    {
        [Get("/api/group/{userId}")]
        Task<IEnumerable<RelationGroupDto>> GetMyGroups(Guid userId);
    }
}
