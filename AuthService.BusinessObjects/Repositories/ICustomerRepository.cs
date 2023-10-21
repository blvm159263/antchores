using System.Collections.Generic;
using AuthService.BusinessObjects.Entities;

namespace AuthService.BusinessObjects.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();

        Customer GetCustomerById(int id);

        void CreateCustomer(Customer cus);

        bool AccountExists(int id);
    }
}