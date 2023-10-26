using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Services.CacheService;
using ProductService.Services.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace ProductService.API.Controllers
{
    [Route("api/p/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private ICacheService _cacheService;

        public CustomerController(IOrderService orderService, IMapper mapper, ICacheService cacheService)
        {
            _orderService = orderService;
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
                var customerItems = _orderService.GetAllCustomers();
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