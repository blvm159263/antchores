using System.Collections.Generic;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories
{
    public interface IOrderRepository
    {
        //Customer
        IEnumerable<Customer> GetAllCustomers();
        void CreateCustomer(Customer cus);
        bool CustomerExists(int cusId);

        bool ExternalCustomerExists(int externalcusId);

        //Order
        IEnumerable<Order> GetAllOrdersForCustomer(int cusId);
        Order GetOrder(int cusId, int oId);
        void CreateOrder(int cusId, Order order);

         //Customer
        IEnumerable<Tasker> GetAllTaskers();
        void CreateTasker(Tasker cus);
        bool TaskerExists(int cusId);

        bool ExternalTaskerExists(int externalcusId);
    }
}