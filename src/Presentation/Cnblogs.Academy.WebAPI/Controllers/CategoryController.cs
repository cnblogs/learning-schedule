using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.CategoryAppService;
using Cnblogs.Academy.Application.CategoryAppService.Dto;
using Cnblogs.Academy.Application.CategoryAppService.InputModel;
using Cnblogs.Academy.WebAPI.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [AdminOnly]
    [Route("api/categories")]
    public class CategoryController : AcademyControllerBase
    {
        private readonly ICategoryService _categorySvc;

        public CategoryController(ICategoryService cateSvc)
        {
            _categorySvc = cateSvc;
        }

        [HttpPost]
        public async Task<long> Post(CategoryInputModel im)
        {
            return await _categorySvc.AddAsync(im);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get()
        {
            return await _categorySvc.GetAllAsync();
        }

        [HttpPut("{id:long}")]
        public async Task Put(long id, CategoryInputModel im)
        {
            await _categorySvc.UpdateAsync(id, im);
        }

        [HttpDelete("{id:long}")]
        public async Task Delete(long id)
        {
            await _categorySvc.DeleteAsync(id);
        }
    }
}
