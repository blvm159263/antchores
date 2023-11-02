using System.Collections.Generic;
using AuthService.Repositories.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services.CacheService;
using AuthService.Services.Services;
using AuthService.Services.SyncDataServices.Http;
using System.Threading.Tasks;
using System;
using AuthService.Services.AsyncDataServices;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.API.Controllers
{
    [Route("api/a/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITaskerService _taskerService;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ICustomerService _customerService;
        private ICacheService _cacheService;

        public AccountController(
                        IAccountService accountService,
                        IMapper mapper,
                        IAuthDataClient authDataClient,
                        IMessageBusClient messageBusClient,
                        ICustomerService customerService,
                        ITaskerService taskerService,
                        ICacheService cacheService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _customerService = customerService;
            _cacheService = cacheService;
            _taskerService = taskerService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<AccountReadModel>> GetAllAccounts()
        {
            string key = "allAccounts";

            var cacheAccounts = _cacheService.GetData<IEnumerable<AccountReadModel>>("allAccounts");

            if (cacheAccounts == null)
            {
                cacheAccounts = _accountService.GetAllAccounts();

                _cacheService.SetData(key, cacheAccounts);

                return Ok(cacheAccounts);
            }

            return Ok(cacheAccounts);
        }

        [HttpGet("{id}", Name = "GetAccountById")]
        public ActionResult<AccountReadModel> GetAccountById(int id)
        {
            string key = $"account-{id}";
            var cacheAccount = _cacheService.GetData<AccountReadModel>(key);


            if (cacheAccount == null)
            {
                cacheAccount = _accountService.GetAccountById(id);

                _cacheService.SetData(key, cacheAccount);

                return Ok(cacheAccount);
            }

            return Ok(cacheAccount);
        }

        /*[HttpPost("{id}/customers")]
        public async Task<ActionResult<CustomerReadModel>> CreateCustomer(int id, CustomerCreateModel customerCreateModel)
        {

            if (!_customerService.AccountExists(id))
                return NotFound();

            var cusRead = _customerService.CreateCustomer(id, customerCreateModel);

            //Send Sync Message
            try
            {
                await _authDataClient.SendCustomerToAuth(cusRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send synchronously!: " + ex.Message);
            }

            //Send Async Message
            try
            {
                var CustomerPublishedModel = _mapper.Map<CustomerPublishedModel>(cusRead);
                CustomerPublishedModel.Event = "Customer_Published";
                _messageBusClient.PublishNewCustomer(CustomerPublishedModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }
            return Created("", cusRead);
        }

        [HttpPost("{id}/taskers")]
        public async Task<ActionResult<TaskerReadModel>> CreateTasker(int id, TaskerCreateModel taskerCreateModel)
        {

            if (!_taskerService.AccountExists(id))
                return NotFound();

            var cusRead = _taskerService.CreateTasker(id, taskerCreateModel);

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
            return Created("", cusRead);
        }*/

        [HttpGet("{id}/customers")]
        public ActionResult<CustomerReadModel> GetCustomerByAccountId(int id)
        {
            var res = _customerService.GetCustomerByAccountId(id);
            
            Console.WriteLine(res);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [HttpGet("{id}/taskers")]
        public ActionResult<TaskerReadModel> GetTaskerByAccountId(int id)
        {
            var res = _taskerService.GetTaskerByAccountId(id);

            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }
    }
}