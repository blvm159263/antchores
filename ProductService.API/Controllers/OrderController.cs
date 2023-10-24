using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using ProductService.Services.Services;
using System.Linq;

namespace ProductService.API.Controllers
{
    [Route("api/c/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("after-date")]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrdersAfterDate(DateTime currentDate) {
            IEnumerable<OrderReadModel> orders = _orderService.GetOrdersAfterDate(currentDate);
            if(orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}