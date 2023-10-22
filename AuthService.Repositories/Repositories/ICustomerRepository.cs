using System.Collections.Generic;
using AuthService.Repositories.Entities;

namespace AuthService.Repositories.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();

        Customer GetCustomerById(int id);

        void CreateCustomer(Customer cus);

        bool AccountExists(int id);
    }
}