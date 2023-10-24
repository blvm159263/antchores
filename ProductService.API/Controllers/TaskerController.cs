using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Services.CacheService;
using ProductService.Services.Services;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ProductService.API.Controllers
{
    [Route("api/c/taskers")]
    [ApiController]
    public class TaskerController : Controller
    {
        private readonly ITaskerService _taskerService;
        private readonly ICacheService _cacheService;

        public TaskerController(ITaskerService taskerService, ICacheService cacheService)
        {
            _taskerService = taskerService;
            _cacheService = cacheService;
        }

        [HttpGet("{taskerId}/cert")]
        public ActionResult<IEnumerable<TaskerCertReadModel>> GetTaskerCertsByTaskerId(int taskerId) {
            string key = $"tasker-{taskerId}-cert";

            IEnumerable<TaskerCertReadModel> taskerCertReadModels = _cacheService.GetData<IEnumerable<TaskerCertReadModel>>(key);

            if(taskerCertReadModels == null)
            {
                taskerCertReadModels = _taskerService.GetTaskerCertsByTaskerId(taskerId);
                if(taskerCertReadModels != null)
                {
                    _cacheService.SetData(key, taskerCertReadModels);
                    return Ok(taskerCertReadModels);
                }
                else
                {
                    NotFound($"Tasker id {taskerId} not have cert!");
                }
            }
            return Ok(taskerCertReadModels);
        }

        [HttpGet("{taskerId}/orders/available")]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrderAvailableForTasker(int taskerId, DateTime time) {
            IEnumerable<OrderReadModel> orders = _taskerService.GetOrdersAvailableOfTasker(taskerId, time);
            if (orders.Count() < 1) return NotFound();

            return Ok(orders);
        }
    }
}