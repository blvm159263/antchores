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
using Microsoft.AspNetCore.Authorization;

namespace AuthService.API.Controllers
{
    [Route("api/a/customers")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Customer")]
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
    }
}