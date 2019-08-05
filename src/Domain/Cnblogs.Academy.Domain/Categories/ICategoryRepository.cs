using System.Linq;
using Cnblogs.Domain.Abstract;

namespace Cnblogs.Academy.Domain.Categories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IQueryable<Category> Categories { get; }

        void Add(Category category);
    }
}
