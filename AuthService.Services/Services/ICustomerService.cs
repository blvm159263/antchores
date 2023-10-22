using AuthService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services
{
    public interface ICustomerService
    {
        IEnumerable<CustomerReadModel> GetAllCustomers();
        CustomerReadModel GetCustomerById(int id);
        CustomerReadModel CreateCustomer(int accountId, CustomerCreateModel customerCreateModel);
        bool AccountExists(int accountId);
    }
}
