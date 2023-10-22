using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Repositories.Data;
using AuthService.Repositories.Entities;

namespace AuthService.Repositories.Repositories.Impl
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AccountExists(int id)
        {
            return _context.Accounts.Any(a => a.Id == id);
        }

        public void CreateCustomer(Customer customer)
        {
            if(customer == null)
                throw new ArgumentNullException(nameof(customer));

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }
    }
}