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
        private readonly ICustomerRepository _customerRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
            _customerRepository = customerRepository;
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

        public IEnumerable<OrderReadModel> GetOrdersByStateAndAfterDate(OrderEnum state, DateTime currentDate)
        {
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            var orders = _orderRepository.GetOrdersAfterDate(currentDate);
            orders = orders.Where(o => o.State.Equals(state)).OrderBy(o => o.StartTime);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }

        public IEnumerable<OrderReadModel> GetOrdersByStateAndAfterDateByCustomerId(OrderEnum state, DateTime currentDate, int customerId)
        {
            var customer = _customerRepository.GetCustomerById(customerId);
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            var orders = _orderRepository.GetOrdersAfterDate(currentDate);
            orders = orders
                .Where(o => o.State.Equals(state) && o.CustomerId == customer.Id)
                .OrderBy(o => o.StartTime);
            return _mapper.Map<IEnumerable<OrderReadModel>>(orders);
        }

        public void CreateOrder(CartModel cartModel)
        {
            Order order = new Order
            {
                CreatedAt = DateTime.Now,
                WorkingAt = cartModel.WorkingAt,
                StartTime = cartModel.StartTime,
                PaymentStatus = PaymentStatus.Pending,
                Total = 0,
                State = OrderEnum.Pending,
                CustomerId = cartModel.CustomerId,
            };

            _orderRepository.CreateOrder(cartModel.CustomerId, order);

            decimal total = 0;

            foreach (var item in cartModel.CartItems)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    TaskDetailId = item.TaskDetailId,
                    Quantity = item.Quantity,
                    Status = true
                };

                if(item.Unit == QuantityUnit.M2)
                {
                    total += item.Price;
                }
                else
                {
                    total += item.Quantity * item.Price;
                }

                _orderDetailRepository.CreateOrderDetail(orderDetail);
            }

            order.Total = total;

            var res = _orderRepository.UpdateOrder(order);

            if (!res)
            {
                throw new Exception("Create order failed");
            }
        }
    }
}
