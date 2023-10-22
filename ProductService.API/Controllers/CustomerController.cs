using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Services.CacheService;
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
        private ICacheService _cacheService;

        public CustomerController(IOrderRepository orderRepository, IMapper mapper, ICacheService cacheService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomers(){
            Console.WriteLine("Get Customers from product service...");
            string key = "allCustomersProduct";
            var cacheCustomers = _cacheService.GetData<IEnumerable<CustomerReadModel>>(key);

            if (cacheCustomers == null)
            {
                var customerItems = _orderRepository.GetAllCustomers();
                cacheCustomers = _mapper.Map<IEnumerable<CustomerReadModel>>(customerItems);

                _cacheService.SetData(key, cacheCustomers);
                return Ok(cacheCustomers);
            }

            return Ok(cacheCustomers);
        }

        [HttpPost]
        public ActionResult Test(){
            Console.WriteLine("Test...");

            return Ok("Testing from Customer Controller");
        }
    }
}