using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories
{
    public interface ITaskDetailRepository
    {
        IEnumerable<TaskDetail> GetAllTaskDetails();
        TaskDetail GetTaskDetailById(int id);
        TaskDetail CreateTaskDetail(TaskDetail taskDetail);
        TaskDetail UpdateTaskDetail(TaskDetail taskDetail);
        TaskDetail DeleteTaskDetail(int id);
        IEnumerable<TaskDetail> GetTaskDetailsByCategoryId(int id);
    }
}
