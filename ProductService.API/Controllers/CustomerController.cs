using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace ProductService.API.Controllers
{
    [Route("api/c/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private IDistributedCache _distributedCache;

        public CustomerController(IOrderRepository orderRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomers(){
            Console.WriteLine("Get Customers from product service...");

            string? cacheCustomers = _distributedCache.GetString("allCustomers");

            IEnumerable<CustomerReadModel>? customerReadModels;

            if (string.IsNullOrEmpty(cacheCustomers))
            {
                var customerItems = _orderRepository.GetAllCustomers();
                customerReadModels = _mapper.Map<IEnumerable<CustomerReadModel>>(customerItems);

                _distributedCache.SetString("allCustomers", JsonSerializer.Serialize(customerReadModels));
                return Ok(customerReadModels);
            }

            customerReadModels = JsonSerializer.Deserialize<IEnumerable<CustomerReadModel>>(cacheCustomers);

            return Ok(customerReadModels);
        }

        [HttpPost]
        public ActionResult Test(){
            Console.WriteLine("Test...");

            return Ok("Testing from Customer Controller");
        }
    }
}