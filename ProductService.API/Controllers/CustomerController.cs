using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProductService.API.Controllers
{
    [Route("api/c/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public CustomerController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomers(){
            Console.WriteLine("Get Customers from product service...");
            var customerItems = _orderRepository.GetAllCustomers();
            return Ok(_mapper.Map<IEnumerable<CustomerReadModel>>(customerItems));
        }

        [HttpPost]
        public ActionResult Test(){
            Console.WriteLine("Test...");

            return Ok("Testing from Customer Controller");
        }
    }
}