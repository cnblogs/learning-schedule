using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Dto;

namespace Cnblogs.Academy.Application.Queries
{
    public interface ICategoryQueries
    {
        Task<IEnumerable<CategoryDto>> GetTrailOf(long id);
    }
}
