using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using ProductService.Services.Services;
using System.Linq;
using ProductService.Services.CacheService;

namespace ProductService.API.Controllers
{
    [Route("api/p/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICacheService _cacheService;

        public OrderController(IOrderService orderService, ICacheService cacheService)
        {
            _orderService = orderService;
            _cacheService = cacheService;
        }

        [HttpGet("available")]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrdersAfterDate()
        {
            IEnumerable<OrderReadModel> orders = _orderService.GetOrdersAfterDate(DateTime.Now);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("{orderId}/orderdetails")]
        public ActionResult<IEnumerable<OrderDetailReadModel>> GetOrderDetailsByOrderId(int orderId)
        {
            string key = $"orderDetails-orderId{orderId}";

            var orderdetail = _cacheService.GetData<IEnumerable<OrderDetailReadModel>>(key);
            if (orderdetail == null)
            {
                orderdetail = _orderService.GetDetailByOrderId(orderId);
                _cacheService.SetData(key, orderdetail);
                return Ok(orderdetail);
            }

            return Ok(orderdetail);
        }

        [HttpGet("{orderId}")]
        public ActionResult<OrderReadModel> GetOrderByOrderId(int orderId)
        {
            string key = $"order-orderId{orderId}";
            var order = _cacheService.GetData<OrderReadModel>(key);
            if (order == null)
            {
                order = _orderService.GetOrderByOrderId(orderId);
                _cacheService.SetData(key, order);
                return Ok(order);
            }

            return Ok(order);

        }
    }
}