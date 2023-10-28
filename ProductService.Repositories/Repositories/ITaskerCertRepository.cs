using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories
{
    public interface ITaskerCertRepository
    {
        void AddCategoryForTasker(TaskerCert taskerCert);
        IEnumerable<TaskerCert> GetAllTaskerCertByTaskerId(int taskerId);
    }
}
