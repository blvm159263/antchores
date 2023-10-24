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
    }
}
