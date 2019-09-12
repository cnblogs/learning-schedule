using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.CategoryAppService.InputModel;
using Cnblogs.Academy.Dto;

namespace Cnblogs.Academy.Application.CategoryAppService
{
    public interface ICategoryService
    {
        Task<long> AddAsync(CategoryInputModel im);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task UpdateAsync(long id, CategoryInputModel im);
        Task DeleteAsync(long id);
    }
}
