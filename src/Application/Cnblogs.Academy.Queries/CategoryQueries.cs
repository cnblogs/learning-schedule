using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Categories;
using Cnblogs.Academy.Dto;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Application.Queries
{
    public class CategoryQueries : ICategoryQueries
    {
        private readonly ICategoryRepository _repository;

        public CategoryQueries(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> GetTrailOf(long id)
        {
            var categories = new List<CategoryDto>();
            await ById(id, categories);
            categories.Reverse();
            return categories;
        }

        private async Task ById(long id, IList<CategoryDto> categories)
        {
            var category = await _repository.Categories.Where(x => x.Id == id).ProjectToType<CategoryDto>().FirstOrDefaultAsync();
            if (category != null)
            {
                categories.Add(category);
                if (category.ParentId > 0)
                {
                    await ById(category.ParentId, categories);
                }
            }
        }
    }
}
