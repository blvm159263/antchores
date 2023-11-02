using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Enums;

namespace ProductService.Repositories.Repositories.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CustomerExists(int cusId)
        {
            return _context.Customers.Any(c => c.Id == cusId);
        }

        public void CreateCustomer(Customer cus)
        {
            if (cus == null)
            {
                throw new ArgumentNullException(nameof(cus));
            }
            _context.Customers.Add(cus);
            _context.SaveChanges();
        }

        public void CreateOrder(int cusId, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.CustomerId = cusId;
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public bool ExternalCustomerExists(int externalcusId)
        {
            return _context.Customers.Any(c => c.ExternalId == externalcusId);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public IEnumerable<Order> GetAllOrdersForCustomer(int cusId)
        {
            return _context.Orders.Where(t => t.CustomerId == cusId)
            .OrderBy(t => t.Id)
            .ToList();
        }

        public Order GetOrder(int cusId, int orderId)
        {
            return _context.Orders.FirstOrDefault(t => t.CustomerId == cusId && t.Id == orderId);
        }

        public IEnumerable<Tasker> GetAllTaskers()
        {
            return _context.Taskers.ToList();
        }

        public void CreateTasker(Tasker tasker)
        {
            if (tasker == null)
            {
                throw new ArgumentNullException(nameof(tasker));
            }
            _context.Taskers.Add(tasker);
            _context.SaveChanges();
        }

        public bool TaskerExists(int taskerId)
        {
            return _context.Taskers.Any(c => c.Id == taskerId);
        }

        public bool ExternalTaskerExists(int externalTaskerId)
        {
            return _context.Taskers.Any(c => c.ExternalId == externalTaskerId);
        }

        public IEnumerable<Order> GetOrdersAfterDate(DateTime currentDate)
        {
            return _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.TaskDetail)
                .ThenInclude(td => td.Category)
                .Where(o => o.StartTime > currentDate);
        }

        public IEnumerable<Order> GetApproriateOrderByCategoriesOfTasker(int taskerId)
        {
            IEnumerable<TaskerCert> taskerCerts = _context.TaskerCerts.Where(tc => tc.TaskerId == taskerId);
            IList<int> categoryIds = taskerCerts.Select(tc => tc.CategoryId).ToList();

            IEnumerable<Order> approriateOrders = _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.TaskDetail)
                .ThenInclude(td => td.Category)
                .Where(
                    o => o.OrderDetails.Any(od => categoryIds.Contains(od.TaskDetail.CategoryId)) &&
                    (
                        o.StartTime > DateTime.Now ||
                        (o.StartTime == DateTime.Now && o.StartTime.Hour > DateTime.Now.Hour + 3)
                    )
                );

            return approriateOrders;
        }

        public Order GetOrderByOrderId(int orderId)
        => _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.TaskDetail)
                .ThenInclude(td => td.Category)
                .FirstOrDefault(o => o.Id == orderId);

        public bool UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Order> GetOrdersByState(OrderEnum state)
        => _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.TaskDetail)
                .ThenInclude(td => td.Category)
                .Where(o => o.State == state);
    }
}