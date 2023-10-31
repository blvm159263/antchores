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
    [Route("api/p/taskers")]
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

        [HttpGet("{id}")]
        public ActionResult<TaskerModel> GetTaskerById(int id)
        {
            string key = $"tasker-{id}";

            TaskerModel taskerModel = _cacheService.GetData<TaskerModel>(key);

            if(taskerModel == null)
            {
                taskerModel = _taskerService.GetTaskerById(id);
                if(taskerModel != null)
                {
                    _cacheService.SetData(key, taskerModel);
                    return Ok(taskerModel);
                }
                else
                {
                    NotFound($"Tasker id {id} not found!");
                }
            }
            return Ok(taskerModel);
        }

        [HttpGet("{id}/certs")]
        public ActionResult<IEnumerable<TaskerCertReadModel>> GetTaskerCertsByTaskerId(int id) {
            string key = $"tasker-{id}-cert";

            IEnumerable<TaskerCertReadModel> taskerCertReadModels = _cacheService.GetData<IEnumerable<TaskerCertReadModel>>(key);

            if(taskerCertReadModels == null)
            {
                taskerCertReadModels = _taskerService.GetTaskerCertsByTaskerId(id);
                if(taskerCertReadModels != null)
                {
                    _cacheService.SetData(key, taskerCertReadModels);
                    return Ok(taskerCertReadModels);
                }
                else
                {
                    NotFound($"Tasker id {id} not have cert!");
                }
            }
            return Ok(taskerCertReadModels);
        }

        [HttpGet("{id}/orders/available")]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrderAvailableForTasker(int id, DateTime time) {
            Console.WriteLine(time);
            if(time == null) time = DateTime.Now;
            IEnumerable<OrderReadModel> orders = _taskerService.GetOrdersAvailableOfTasker(id, time);
            if (orders.Count() < 1) return NotFound();

            return Ok(orders);
        }

        [HttpPut("{id}/categories")]
        public ActionResult<TaskerModel> AddCategoryServiceForTasker(int id, [FromBody] List<int> categoryIds)
        {
            TaskerModel taskerModel = _taskerService.AddCategoryServiceForTasker(id, categoryIds);

            if (taskerModel == null) return NotFound();

            _cacheService.RemoveData($"tasker-{id}-cert");

            _cacheService.RemoveData($"tasker-{id}");

            return Ok(taskerModel);
        }

        [HttpPost("{taskerId}/order/{orderId}")]
        public ActionResult<string> CreateContract(int orderId, int taskerId) { 
            ContractCreateModel contractCreateModel = new ContractCreateModel();
            contractCreateModel.OrderId = orderId;
            contractCreateModel.TaskerId = taskerId;
            bool result = _taskerService.CreateContract(contractCreateModel);
            if(result)
            {
                return Ok($"Contact create successful for tasker {taskerId} with order {orderId}");
            }
            return BadRequest("Cannot create! Check again!");
        }
    }
}