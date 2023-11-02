using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories.Impl
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Customer GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.ExternalId == id);
        }
    }
}
