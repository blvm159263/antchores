using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services
{
    public interface ITaskDetailService
    {
        IEnumerable<TaskDetailReadModel> GetAllTaskDetails();
        TaskDetailReadModel GetTaskDetailById(int id);
        TaskDetailReadModel CreateTaskDetail(TaskDetailCreateModel taskDetailCreateModel);
        TaskDetailReadModel UpdateTaskDetail(int id, TaskDetailCreateModel taskDetailCreateModel);
        TaskDetailReadModel DeleteTaskDetail(int id);
        IEnumerable<TaskDetailReadModel> GetTaskDetailsByCategoryId(int id);
    }
}
