using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Services.CacheService;
using ProductService.Services.Services;

namespace ProductService.API.Controllers
{
    [Route("api/p/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITaskDetailService _taskDetailService;
        private readonly ICacheService _cacheService;

        public CategoriesController(ICategoryService categoryService, ICacheService cacheService, ITaskDetailService taskDetailService)
        {
            _categoryService = categoryService;
            _cacheService = cacheService;
            _taskDetailService = taskDetailService;
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

            if(categoryItem == null)
            {
                return BadRequest();
            }

            refreshCache(categoryItem.Id);

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryItem.Id }, categoryItem);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(CategoryCreateModel categoryCreateModel, int id)
        {
            var categoryItem = _categoryService.UpdateCategory(categoryCreateModel, id);

            refreshCache(id);

            return Ok(categoryItem);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryItem =  _categoryService.DeleteCategory(id);

            refreshCache(id);

            return Ok(categoryItem);
        }

        [HttpGet("{id}/task-details")]
        public IActionResult GetTaskDetailsByCategoryId(int id)
        {
            var cacheKey = $"taskDetails-{id}";

            var cacheTaskDetails = _cacheService.GetData<IEnumerable<TaskDetailReadModel>>(cacheKey);

            if (cacheTaskDetails == null)
            {
                var taskDetails = _taskDetailService.GetTaskDetailsByCategoryId(id);

                _cacheService.SetData(cacheKey, taskDetails);

                return Ok(taskDetails);
            }

            return Ok(cacheTaskDetails);
        }

        #region refresh cache
        private void refreshCache(int id)
        {
            string getAllKey = "allCategories";

            string getByIdKey = $"category-{id}";

            _cacheService.RemoveData(getAllKey);

            _cacheService.RemoveData(getByIdKey);
        }
        #endregion
    }
}
