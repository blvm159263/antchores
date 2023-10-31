using System.Collections.Generic;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories
{
    public interface IContractRepository
    {
        IEnumerable<Contract> GetContractsByTaskerId(int taskerId);

        bool CreateContact(Contract con);

        Contract GetContractsByTaskerIdAndOrderId(int taskerId, int orderId);
    }
}