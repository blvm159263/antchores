using AuthService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services
{
    public interface ITaskerService
    {
        IEnumerable<TaskerReadModel> GetAllTaskers();
        TaskerReadModel GetTaskerById(int id);
        TaskerReadModel CreateTasker(int accountId, TaskerCreateModel taskerCreateModel);
        bool AccountExists(int accountId);
    }
}
