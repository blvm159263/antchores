using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;

namespace ProductService.API.Controllers
{
    [Route("api/c/customers/{customerId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository OrderRepository, IMapper mapper)
        {
            _orderRepository = OrderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderReadModel>> GetOrdersForcustomer(int customerId){
            Console.WriteLine($"Hit GetOrdersForcustomer from auth service: {customerId}");
            if(!_orderRepository.CustomerExists(customerId)){
                return NotFound();
            }
            
            var orders = _orderRepository.GetAllOrdersForCustomer(customerId);
            return Ok(_mapper.Map<IEnumerable<OrderReadModel>>(orders)); 
        }


        [HttpGet("{orderId}", Name="GetOrderForCustomer")]
        public ActionResult<OrderReadModel> GetOrderForCustomer(int customerId, int orderId){
            Console.WriteLine($"Hit GetOrderForCustomer from auth service: {customerId}, OrderId: {orderId}");
            if(!_orderRepository.CustomerExists(customerId)){
                return NotFound();
            }

            var order = _orderRepository.GetOrder(customerId, orderId);
            if(order == null){
                return NotFound();
            }

            return Ok(_mapper.Map<OrderReadModel>(order));
        }

        [HttpPost]
        public ActionResult<OrderReadModel> CreateOrderForPlatform(int customerId, OrderCreateModel orderCreateModel){
            Console.WriteLine($"Hit CreateOrderForPlatform from auth service: {customerId}");
            if(!_orderRepository.CustomerExists(customerId)){
                return NotFound();
            }

            var Order = _mapper.Map<Order>(orderCreateModel);
            _orderRepository.CreateOrder(customerId, Order);

            var OrderReadModel = _mapper.Map<OrderReadModel>(Order);
            return CreatedAtRoute(nameof(
                GetOrderForCustomer), new {customerId = customerId, orderId = OrderReadModel.Id}, OrderReadModel);
        }
    }
}