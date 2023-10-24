using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Repositories.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public void CreateCustomer(Customer cus) 
            => _orderRepository.CreateCustomer(cus);

        public void CreateTasker(Tasker cus)
        => _orderRepository.CreateTasker(cus);

        public bool CustomerExists(int cusId)
        => _orderRepository.CustomerExists(cusId);

        public bool ExternalCustomerExists(int externalcusId)
        => _orderRepository.ExternalCustomerExists(externalcusId);

        public bool ExternalTaskerExists(int externalcusId)
        => _orderRepository.ExternalTaskerExists(externalcusId);

        public IEnumerable<Customer> GetAllCustomers()
        => _orderRepository.GetAllCustomers();


        public IEnumerable<Tasker> GetAllTaskers()
        => _orderRepository.GetAllTaskers();

        public bool TaskerExists(int cusId)
        => _orderRepository.TaskerExists(cusId);

        public IEnumerable<OrderReadModel> GetOrdersAfterDate(DateTime currentDate)
        {
            IEnumerable<Order> orders = _orderRepository.GetOrdersAfterDate(currentDate);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }
    }
}
