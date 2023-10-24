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

namespace AuthService.API.Controllers
{
    [Route("api/taskers")]
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

        [HttpGet]
        public ActionResult<IEnumerable<TaskerReadModel>> GetAllTaskers()
        {
            string key = "allTaskers";
         
            var cacheTaskers = _cacheService.GetData<IEnumerable<TaskerReadModel>>(key);

            if (cacheTaskers == null)
            {
                var taskers = _taskerService.GetAllTaskers();

                _cacheService.SetData(key, taskers);

                return Ok(cacheTaskers);
            }

            return Ok(cacheTaskers);
        }


        [HttpGet("{id}", Name = "GetTaskerById")]
        public ActionResult<IEnumerable<TaskerReadModel>> GetTaskerById(int id)
        {
            string key = $"tasker-{id}";

            var cacheTasker = _cacheService.GetData<TaskerReadModel>(key);

            if (cacheTasker == null)
            {
                var tasker = _taskerService.GetTaskerById(id);

                _cacheService.SetData(key , tasker);

                return Ok(tasker);
            }

            return Ok(cacheTasker);
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<TaskerReadModel>> CreateTasker(
            int accountId, TaskerCreateModel taskerCreateModel)
        {

            if (!_taskerService.AccountExists(accountId))
                return NotFound();

            /*var cusModel = _mapper.Map<Tasker>(taskerCreateModel);
            cusModel.AccountId = accountId;
            _taskerRepository.CreateTasker(cusModel);*/

            var cusRead = _taskerService.CreateTasker(accountId, taskerCreateModel);

           /* var cusRead = _mapper.Map<TaskerReadModel>(cusModel);*/

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