using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services.AsyncDataServices;
using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AuthService.Services.SyncDataServices.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AuthService.API.Controllers
{
    [Route("api/taskers")]
    [ApiController]
    public class TaskerController : ControllerBase
    {
        private readonly ITaskerRepository _taskerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private IDistributedCache _distributedCache;

        public TaskerController(
                ITaskerRepository TaskerRepository,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient,
                IDistributedCache distributedCache)
        {
            _taskerRepository = TaskerRepository;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskerReadModel>> GetAllTaskers()
        {
            string? cacheTaskers = _distributedCache.GetString("allTaskers");
            IEnumerable<TaskerReadModel>? taskerReadModels;

            if (string.IsNullOrEmpty(cacheTaskers))
            {
                var taskers = _taskerRepository.GetAll();

                taskerReadModels = _mapper.Map<IEnumerable<TaskerReadModel>>(taskers);

                _distributedCache.SetString("allTaskers", JsonSerializer.Serialize(taskerReadModels));

                return Ok(taskerReadModels);

            }

            taskerReadModels = JsonSerializer.Deserialize<IEnumerable<TaskerReadModel>>(cacheTaskers);
            return Ok(taskerReadModels);
        }


        [HttpGet("{id}", Name = "GetTaskerById")]
        public ActionResult<IEnumerable<TaskerReadModel>> GetTaskerById(int id)
        {
            string key = $"tasker-{id}";
            string? cacheTasker = _distributedCache.GetString(key);

            TaskerReadModel? taskerReadModel;
            if (string.IsNullOrEmpty(cacheTasker))
            {
                var tasker = _taskerRepository.GetTaskerById(id);
                taskerReadModel = _mapper.Map<TaskerReadModel>(tasker);

                _distributedCache.SetString(key, JsonSerializer.Serialize(taskerReadModel));

                return Ok(taskerReadModel);
            }

            taskerReadModel = JsonSerializer.Deserialize<TaskerReadModel>(cacheTasker);
            return Ok(taskerReadModel);
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<TaskerReadModel>> CreateTasker(int accountId, TaskerCreateModel taskerCreateModel)
        {

            if (!_taskerRepository.AccountExists(accountId))
                return NotFound();


            var cusModel = _mapper.Map<Tasker>(taskerCreateModel);
            cusModel.AccountId = accountId;
            _taskerRepository.CreateTasker(cusModel);

            var cusRead = _mapper.Map<TaskerReadModel>(cusModel);

            // //Send Sync Message
            // try
            // {
            //     await _authDataClient.SendTaskerToAuth(cusRead);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine("Could not send synchronously!: " + ex.Message);
            // }

            //Send Async Message
            try
            {
                var taskerPublishedModel = _mapper.Map<TaskerPublishedModel>(cusRead);
                taskerPublishedModel.Event = "Tasker_Published";
                _messageBusClient.PublishNewTasker(taskerPublishedModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }

            return CreatedAtRoute(nameof(GetTaskerById), new { Id = cusRead.Id }, cusRead);
        }
    }
}