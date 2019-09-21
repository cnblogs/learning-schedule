using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.CategoryAppService.Dto;
using Cnblogs.Academy.Application.CategoryAppService.InputModel;
using Cnblogs.Academy.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System.Linq;
using Enyim.Caching;

namespace Cnblogs.Academy.Application.CategoryAppService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemcachedClient _cache;
        private const string _cacheKey = "academy_categories_all";

        public CategoryService(ICategoryRepository categoryRepository, IMemcachedClient memcached)
        {
            _categoryRepository = categoryRepository;
            _cache = memcached;
        }

        public async Task<long> AddAsync(CategoryInputModel im)
        {
            var category = new Category(im.Name, im.ParentId);
            _categoryRepository.Add(category);
            await _categoryRepository.UnitOfWork.SaveChangesAsync();
            _cache.Remove(_cacheKey);
            return category.Id;
        }

        public async Task DeleteAsync(long id)
        {
            var source = await _categoryRepository.Categories.ToListAsync();
            var categories = GetCategories(id, source);
            categories.Add(source.FirstOrDefault(x => x.Id == id));
            foreach (var category in categories)
            {
                category.Delete();
            }
            await _categoryRepository.UnitOfWork.SaveChangesAsync();
            _cache.Remove(_cacheKey);
        }

        private IList<Category> GetCategories(long id, List<Category> source)
        {
            var matched = new List<Category>();
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].ParentId == id)
                {
                    matched.Add(source[i]);
                    matched.AddRange(GetCategories(source[i].Id, source));
                }
            }
            return matched;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _cache.GetValueOrCreateAsync(_cacheKey, 24 * 60 * 60,
             async () => await _categoryRepository.Categories.ProjectToType<CategoryDto>().ToListAsync());
        }

        public async Task UpdateAsync(long id, CategoryInputModel im)
        {
            var category = await _categoryRepository.Categories.FirstOrDefaultAsync(x => x.Id == id);
            category.Rename(im.Name);
            await _categoryRepository.UnitOfWork.SaveChangesAsync();
            _cache.Remove(_cacheKey);
        }
    }
}
