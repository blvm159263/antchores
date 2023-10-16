using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.AsyncDataServices;
using AuthService.Entities;
using AuthService.Models;
using AuthService.Repositories;
using AuthService.SyncDataServices.Http;

namespace AuthService.Controllers
{
    [Route("api/taskers")]
    [ApiController]
    public class TaskerController : ControllerBase
    {
        private readonly ITaskerRepository _taskerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public TaskerController(
                ITaskerRepository TaskerRepository,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient)
        {
            _taskerRepository = TaskerRepository;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskerReadModel>> GetAllTaskers()
        {
            var taskers = _taskerRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<TaskerReadModel>>(taskers));
        }


        [HttpGet("{id}", Name = "GetTaskerById")]
        public ActionResult<IEnumerable<TaskerReadModel>> GetTaskerById(int id)
        {
            var tasker = _taskerRepository.GetTaskerById(id);
            return Ok(_mapper.Map<TaskerReadModel>(tasker));
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<TaskerReadModel>> CreateTasker(int accountId, TaskerCreateModel taskerCreateModel)
        {

            if(!_taskerRepository.AccountExists(accountId))
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
            catch(Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }

            return CreatedAtRoute(nameof(GetTaskerById), new { Id = cusRead.Id }, cusRead);
        }
    }
}