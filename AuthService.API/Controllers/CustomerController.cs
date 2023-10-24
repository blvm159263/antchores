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
using AuthService.Services.CacheService;
using AuthService.Services.Services;

namespace AuthService.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private ICacheService _cacheService;

        public CustomerController(
                ICustomerService customerService,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient,
                ICacheService cacheService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _cacheService = cacheService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetAllCustomers()
        {
            string key = "allCustomers";

            var cacheCustomers = _cacheService.GetData<IEnumerable<CustomerReadModel>>(key);

            if (cacheCustomers == null)
            {
                cacheCustomers = _customerService.GetAllCustomers();

                _cacheService.SetData(key, cacheCustomers);
                
                return Ok(cacheCustomers);
            }
            
            return Ok(cacheCustomers);
        }


        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<CustomerReadModel> GetCustomerById(int id)
        {
            string key = $"customer-{id}";

            var cacheCustomer = _cacheService.GetData<CustomerReadModel>(key);

            if(cacheCustomer == null)
            {
                cacheCustomer = _customerService.GetCustomerById(id);

                _cacheService.SetData(key, cacheCustomer);

                return Ok(cacheCustomer);
            }

            return Ok(cacheCustomer);
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<CustomerReadModel>> CreateCustomer(int accountId, CustomerCreateModel customerCreateModel)
        {

            if(!_customerService.AccountExists(accountId))
                return NotFound();
                

            /*var cusModel = _mapper.Map<Customer>(customerCreateModel);
            cusModel.AccountId = accountId;
            _customerRepository.CreateCustomer(cusModel);*/

            var cusRead = _customerService.CreateCustomer(accountId, customerCreateModel);

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
            catch(Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }

            return CreatedAtRoute(nameof(GetCustomerById), new { Id = cusRead.Id }, cusRead);
        }
    }
}