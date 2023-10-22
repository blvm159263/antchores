using System.Collections.Generic;
using ProductService.Repositories.Entities;

namespace ProductService.Services.SyncDataServices.Grpc
{
    public interface ICustomerDataClient
    {
        IEnumerable<Customer> ReturnAllCustomers();

        IEnumerable<Tasker> ReturnAllTaskers();
    }
}