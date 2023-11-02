using ProductService.Repositories.Entities;
using ProductService.Repositories.Enums;
using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services
{
    public interface IOrderService
    {
        //Customer
        IEnumerable<Customer> GetAllCustomers();
        void CreateCustomer(Customer cus);
        bool CustomerExists(int cusId);

        bool ExternalCustomerExists(int externalcusId);

        //Order
        IEnumerable<OrderReadModel> GetOrdersAfterDate(DateTime currentDate);
        IEnumerable<OrderReadModel> GetApproriateOrderByCategoriesOfTasker(int taskerId);
        OrderReadModel GetOrderByOrderId(int orderId);
        IEnumerable<OrderReadModel> GetOrdersByStateAndAfterDate(OrderEnum state,DateTime currentDate);

        //Customer
        IEnumerable<Tasker> GetAllTaskers();
        void CreateTasker(Tasker cus);
        bool TaskerExists(int cusId);

        bool ExternalTaskerExists(int externalcusId);

        //Order detail
        IEnumerable<OrderDetailReadModel> GetDetailByOrderId(int orderId);
    }
}
