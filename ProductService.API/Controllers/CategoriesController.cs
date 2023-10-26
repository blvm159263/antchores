using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Services.CacheService;
using ProductService.Services.Services;
using ProductService.Services.Services.Impl;
using System.Collections.Generic;

namespace ProductService.API.Controllers
{
    [Route("api/p/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICacheService _cacheService;

        public CategoriesController(ICategoryService categoryService, ICacheService cacheService)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            string key = "allCategories";

            var cacheCategories = _cacheService.GetData<IEnumerable<CategoryReadModel>>(key);

            if (cacheCategories == null)
            {
                var categoryItems = _categoryService.GetAllCategories();

                _cacheService.SetData(key, categoryItems);

                return Ok(categoryItems);
            }

            return Ok(cacheCategories);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            string key = $"category-{id}";

            var cacheCategory = _cacheService.GetData<CategoryReadModel>(key);

            if (cacheCategory == null)
            {
                var categoryItem = _categoryService.GetCategoryById(id);

                if (categoryItem != null)
                {
                    _cacheService.SetData(key, categoryItem);

                    return Ok(categoryItem);
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok(cacheCategory);
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryCreateModel categoryCreateModel)
        {
            var categoryItem = _categoryService.AddCategory(categoryCreateModel);

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryItem.Id }, categoryItem);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(CategoryCreateModel categoryCreateModel, int id)
        {
            var categoryItem = _categoryService.UpdateCategory(categoryCreateModel, id);

            return Ok(categoryItem);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryItem =  _categoryService.DeleteCategory(id);

            return Ok(categoryItem);
        }
    }
}
