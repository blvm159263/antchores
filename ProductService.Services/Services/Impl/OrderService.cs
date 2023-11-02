using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Enums;
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
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
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
            IEnumerable<Order> orders = _orderRepository.GetOrdersAfterDate(currentDate).OrderBy(o => o.StartTime);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }

        public IEnumerable<OrderReadModel> GetApproriateOrderByCategoriesOfTasker(int taskerId)
        {
            IEnumerable<Order> orders = _orderRepository.GetApproriateOrderByCategoriesOfTasker(taskerId);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }

        public IEnumerable<OrderDetailReadModel> GetDetailByOrderId(int orderId)
        {
            var od = _orderDetailRepository.GetByOrderId(orderId);
            return _mapper.Map<IEnumerable<OrderDetailReadModel>>(od);
        }

        public OrderReadModel GetOrderByOrderId(int orderId)
        {
            var order = _orderRepository.GetOrderByOrderId(orderId);
            return _mapper.Map<OrderReadModel>(order);
        }

        public IEnumerable<OrderReadModel> GetOrdersByStateAndAfterDate(OrderEnum state,DateTime currentDate){
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            var orders = _orderRepository.GetOrdersAfterDate(currentDate);
            orders = orders.Where(o => o.State.Equals(state)).OrderBy(o => o.StartTime);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }
    }
}
