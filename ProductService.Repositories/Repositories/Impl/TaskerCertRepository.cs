using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories.Impl
{
    public class TaskerCertRepository : ITaskerCertRepository
    {
        private readonly AppDbContext _context;

        public TaskerCertRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddCategoryForTasker(TaskerCert taskerCert)
        {
            _context.TaskerCerts.Add(taskerCert);
            _context.SaveChanges();
        }

        public IEnumerable<TaskerCert> GetAllTaskerCertByTaskerId(int taskerId)
        {
            return _context.TaskerCerts.Where(x => x.TaskerId == taskerId).ToList();
        }
    }
}
