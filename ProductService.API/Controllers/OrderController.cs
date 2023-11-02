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
using ProductService.Repositories.Enums;

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

        [HttpGet("state/{state}/available")]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrdersByStateAndAfterDate(int state)
        {
            string key = $"order-state{state}";
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
                orders = _orderService.GetOrdersByStateAndAfterDate(orderEnum, DateTime.Now);
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