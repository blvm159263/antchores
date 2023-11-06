using AuthService.Repositories.Data;
using AuthService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories.Repositories
{
    public class CustomerRepository
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
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public bool UpdateCustomer(Customer customer){
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            _context.ChangeTracker.Clear();
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.SingleOrDefault(c => c.Id == id);
        }

        public Customer GetCustomerByAccountId(int accountId)
        {
            var customer = _context.Customers.SingleOrDefault(x => x.AccountId == accountId);
            return customer;
        }
    }
}
