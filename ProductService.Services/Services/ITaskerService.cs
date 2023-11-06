using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services
{
    public interface ITaskerService
    {
        IEnumerable<TaskerCertReadModel> GetTaskerCertsByTaskerId(int taskerId);

        IEnumerable<OrderReadModel> GetOrdersAvailableOfTasker(int taskerId, DateTime time);
        IEnumerable<OrderReadModel> GetOrdersInProgressForTasker(int taskerId, DateTime time);

        TaskerModel AddCategoryServiceForTasker(int taskerId, List<int> categoryIds);

        TaskerModel GetTaskerById(int taskerId);

        bool CreateContract(ContractCreateModel contractCreateModel);

        bool IsContractExist(ContractCreateModel contractCreateModel);

        bool DeleteContract(int taskerId, int orderId);
    }
}
