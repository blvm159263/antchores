using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories.Impl
{
    public class TaskDetailRepository : ITaskDetailRepository
    {
        private readonly AppDbContext _context;

        public TaskDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public TaskDetail CreateTaskDetail(TaskDetail taskDetail)
        {
            _context.TaskDetails.Add(taskDetail);
            _context.SaveChanges();

            return taskDetail;
        }

        public TaskDetail DeleteTaskDetail(int id)
        {
            var taskDetail = GetTaskDetailById(id);

            if (taskDetail != null)
            {
                _context.TaskDetails.Remove(taskDetail);
                _context.SaveChanges();
            }

            return taskDetail;
        }

        public IEnumerable<TaskDetail> GetAllTaskDetails()
        {
            return _context.TaskDetails.ToList();
        }

        public TaskDetail GetTaskDetailById(int id)
        {
            return _context.TaskDetails.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<TaskDetail> GetTaskDetailsByCategoryId(int id)
        {
            return _context.TaskDetails.Where(p => p.CategoryId == id).ToList();
        }

        public TaskDetail UpdateTaskDetail(TaskDetail taskDetail)
        {
            _context.Entry(taskDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();

            return taskDetail;
        }
    }
}
