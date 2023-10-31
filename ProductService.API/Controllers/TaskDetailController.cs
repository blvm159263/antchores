using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Services.CacheService;
using ProductService.Services.Services;

namespace ProductService.API.Controllers
{
    [Route("api/p/task-details")]
    [ApiController]
    public class TaskDetailController : Controller
    {
        private readonly ITaskDetailService _taskDetailService;
        private readonly ICacheService _cacheService;
        
        public TaskDetailController(ITaskDetailService taskDetailService, ICacheService cacheService)
        {
            _taskDetailService = taskDetailService;
            _cacheService = cacheService;
        }
        
        [HttpPost("")]
        public IActionResult AddTaskDetail(TaskDetailCreateModel taskDetailCreateModel)
        {
            var taskDetailItem = _taskDetailService.CreateTaskDetail(taskDetailCreateModel);

            if (taskDetailItem == null)
            {
                return NoContent();
            }

            var cacheKey = $"taskDetails-{taskDetailCreateModel.CategoryId}";
            _cacheService.RemoveData(cacheKey);

            return Created("", taskDetailItem);
        }
        
        [HttpPut("{taskId}")]
        public IActionResult UpdateTaskDetail(TaskDetailCreateModel taskDetailCreateModel, int taskId)
        {
            var taskDetailItem = _taskDetailService.UpdateTaskDetail(taskId, taskDetailCreateModel);

            var cacheKey = $"taskDetails-{taskDetailCreateModel.CategoryId}";
            _cacheService.RemoveData(cacheKey);

            return Ok(taskDetailItem);
        }
    }
}