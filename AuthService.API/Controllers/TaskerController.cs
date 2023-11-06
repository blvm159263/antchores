using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services.AsyncDataServices;
using AuthService.Repositories.Models;
using AuthService.Services.SyncDataServices.Http;
using AuthService.Services.CacheService;
using AuthService.Services.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.API.Controllers
{
    [Route("api/a/taskers")]
    [ApiController]
    public class TaskerController : ControllerBase
    {
        private readonly ITaskerService _taskerService;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private ICacheService _cacheService;

        public TaskerController(
                ITaskerService taskerService,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient,
                ICacheService cacheService)
        {
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _cacheService = cacheService;
            _taskerService = taskerService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<TaskerReadModel>> GetAllTaskers()
        {
            string key = "allTaskers";
         
            var cacheTaskers = _cacheService.GetData<IEnumerable<TaskerReadModel>>(key);

            if (cacheTaskers == null)
            {
                cacheTaskers = _taskerService.GetAllTaskers();

                _cacheService.SetData(key, cacheTaskers);

                return Ok(cacheTaskers);
            }

            return Ok(cacheTaskers);
        }
        
        [Authorize(Roles = "Tasker")]
        [HttpGet("{id}", Name = "GetTaskerById")]
        public ActionResult<IEnumerable<TaskerReadModel>> GetTaskerById(int id)
        {
            string key = $"tasker-{id}";

            var cacheTasker = _cacheService.GetData<TaskerReadModel>(key);

            if (cacheTasker == null)
            {
                cacheTasker = _taskerService.GetTaskerById(id);

                _cacheService.SetData(key , cacheTasker);

                return Ok(cacheTasker);
            }

            return Ok(cacheTasker);
        }

        [Authorize(Roles = "Tasker")]
        [HttpGet("account/{id}")]
        public ActionResult<IEnumerable<TaskerReadModel>> GetTaskerByAccountId(int id)
        {
            string key = $"tasker-accountId-{id}";

            var cacheTasker = _cacheService.GetData<TaskerReadModel>(key);

            if (cacheTasker == null)
            {
                cacheTasker = _taskerService.GetTaskerByAccountId(id);

                _cacheService.SetData(key , cacheTasker);

                return Ok(cacheTasker);
            }

            return Ok(cacheTasker);
        }

        [HttpPut("{id}")]
        public ActionResult<bool> UpdateTasker(int id, AuthRequestTaskerModel model)
        {
            bool result = _taskerService.UpdateTasker(id, model);

            if (result)
            {
                try
                {
                    TaskerPublishedModel taskerPublishedModel = new TaskerPublishedModel
                    {
                        Event = "Tasker_Update_Published",
                        Id = id,
                        Name = model.Name,
                        Address = model.Address,
                        Email = model.Email,
                        Identification = model.Identification,
                        Status = true
                    };
                    _messageBusClient.PublishNewTasker(taskerPublishedModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not send asynchronously!: " + ex.Message);
                }
                return Ok(result);
            }

            return BadRequest("Cannot update tasker");
        }
    }
}