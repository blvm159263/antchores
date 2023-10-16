using System.Collections.Generic;
using AuthService.Entities;

namespace AuthService.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();

        Customer GetCustomerById(int id);

        void CreateCustomer(Customer cus);

        bool AccountExists(int id);
    }
}