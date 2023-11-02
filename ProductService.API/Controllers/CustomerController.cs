using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Repositories.Enums;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Services.CacheService;
using ProductService.Services.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomers()
        {
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

        [HttpGet("{id}/orders/available")]
        public ActionResult GetOrdersByStateAndAfterDate(int state, int id)
        {
            string key = $"customer-order-state{state}-{id}";
            var orders = _cacheService.GetData<IEnumerable<OrderReadModel>>(key);
            if (orders == null)
            {
                OrderEnum orderEnum;
                switch (state)
                {
                    case 0:
                        orderEnum = OrderEnum.Pending;
                        break;
                    case 1:
                        orderEnum = OrderEnum.Accepted;
                        break;
                    case 2:
                        orderEnum = OrderEnum.Completed;
                        break;
                    case 3:
                        orderEnum = OrderEnum.Canceled;
                        break;
                    default:
                        orderEnum = OrderEnum.Pending;
                        break;
                }
                orders = _orderService.GetOrdersByStateAndAfterDateByCustomerId(orderEnum, DateTime.Now, id);
                if (orders == null || orders.Count() < 1)
                {
                    return NotFound();
                }
                _cacheService.SetData(key, orders);
                return Ok(orders);
            }
            return Ok(orders);
        }
    }
}