using System.Collections.Generic;
using ProductService.Entities;

namespace ProductService.SyncDataServices.Grpc
{
    public interface ICustomerDataClient
    {
        IEnumerable<Customer> ReturnAllCustomers();

        IEnumerable<Tasker> ReturnAllTaskers();
    }
}