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
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private IDistributedCache _distributedCache;

        public CustomerController(
                ICustomerRepository customerRepository,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient,
                IDistributedCache distributedCache)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetAllCustomers()
        {
            string? cacheCustomers = _distributedCache.GetString("allCustomers");

            IEnumerable<CustomerReadModel>? customerReadModels;

            if (string.IsNullOrEmpty(cacheCustomers))
            {
                var customers = _customerRepository.GetAll();
                customerReadModels = _mapper.Map<IEnumerable<CustomerReadModel>>(customers);

                _distributedCache.SetString("allCustomers", JsonSerializer.Serialize(customerReadModels));
                return Ok(customerReadModels);
            }

            customerReadModels = JsonSerializer.Deserialize<IEnumerable<CustomerReadModel>>(cacheCustomers);
            
            return Ok(customerReadModels);
        }


        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomerById(int id)
        {
            string key = $"customer-{id}";

            string? cacheCustomer = _distributedCache.GetString(key);

            CustomerReadModel? customerReadModel;
            if(string.IsNullOrEmpty(cacheCustomer))
            {
                var customer = _customerRepository.GetCustomerById(id);
                customerReadModel = _mapper.Map<CustomerReadModel>(customer);

                _distributedCache.SetString(key , JsonSerializer.Serialize(customerReadModel));
                return Ok(customerReadModel);
            }

            customerReadModel = JsonSerializer.Deserialize<CustomerReadModel>(cacheCustomer);
            return Ok(customerReadModel);
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