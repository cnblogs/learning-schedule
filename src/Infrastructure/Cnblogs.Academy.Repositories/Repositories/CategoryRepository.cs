using System.Linq;
using Cnblogs.Academy.Domain.Categories;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Repositories.Repositories
{
    public class CategoryRepository : Repository<AcademyContext, Category>, ICategoryRepository
    {
        public CategoryRepository(AcademyContext context) : base(context)
        {
        }

        public IQueryable<Category> Categories => _context.Categories;

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }
    }
}
