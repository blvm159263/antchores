using System;
using System.Collections.Generic;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Repositories
{
    public interface IOrderRepository
    {
        //Customer
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerByExternalId(int cusId);
        void CreateCustomer(Customer cus);

        void UpdateCustomer(Customer cus);
        bool CustomerExists(int cusId);

        bool ExternalCustomerExists(int externalcusId); 

        //Order
        IEnumerable<Order> GetAllOrdersForCustomer(int cusId);
        Order GetOrder(int cusId, int oId);
        void CreateOrder(int cusId, Order order);
        IEnumerable<Order> GetOrdersAfterDate(DateTime currentDate);
        IEnumerable<Order> GetApproriateOrderByCategoriesOfTasker(int taskerId);
        Order GetOrderByOrderId(int orderId);
        bool UpdateOrder(Order order);
        IEnumerable<Order> GetOrdersByState(OrderEnum state);

        //Tasker
        IEnumerable<Tasker> GetAllTaskers();

        Tasker GetTaskerByExternalId(int taskerId);
        void CreateTasker(Tasker cus);

        void UpdateTasker(Tasker cus);
        bool TaskerExists(int cusId);

        bool ExternalTaskerExists(int externalcusId);
    }
}