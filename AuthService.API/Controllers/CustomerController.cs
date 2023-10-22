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
using AuthService.Services.CacheService;

namespace AuthService.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private ICacheService _cacheService;

        public CustomerController(
                ICustomerRepository customerRepository,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient,
                ICacheService cacheService)
        {
            _customerRepository = customerRepository;
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
                var customers = _customerRepository.GetAll();
                cacheCustomers = _mapper.Map<IEnumerable<CustomerReadModel>>(customers);

                _cacheService.SetData<IEnumerable<CustomerReadModel>>(key, cacheCustomers);
                return Ok(cacheCustomers);
            }
            
            return Ok(cacheCustomers);
        }


        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomerById(int id)
        {
            string key = $"customer-{id}";

            var cacheCustomer = _cacheService.GetData<CustomerReadModel>(key);

            if(cacheCustomer == null)
            {
                var customer = _customerRepository.GetCustomerById(id);
                cacheCustomer = _mapper.Map<CustomerReadModel>(customer);

                _cacheService.SetData(key, cacheCustomer);
                return Ok(cacheCustomer);
            }

            return Ok(cacheCustomer);
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<CustomerReadModel>> CreateCustomer(int accountId, CustomerCreateModel customerCreateModel)
        {

            if(!_customerRepository.AccountExists(accountId))
                return NotFound();
                

            var cusModel = _mapper.Map<Customer>(customerCreateModel);
            cusModel.AccountId = accountId;
            _customerRepository.CreateCustomer(cusModel);

            var cusRead = _mapper.Map<CustomerReadModel>(cusModel);

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