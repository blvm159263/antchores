using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories.Impl
{
    public class TaskerRepository : ITaskerRepository
    {

        private readonly AppDbContext _context;

        public TaskerRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateTasker(Tasker tasker)
        {
            if (tasker == null)
            {
                throw new ArgumentNullException(nameof(tasker));
            }
            _context.Taskers.Add(tasker);
            _context.SaveChanges();
        }

        public IEnumerable<Tasker> GetAllTaskers()
        {
            return _context.Taskers
            .Include(x => x.TaskerCerts)
            .Include(x => x.Contacts);
        }

        public Tasker GetTaskerByExternalId(int externalId)
        {
            return _context.Taskers
            .Include(x => x.TaskerCerts)
            .Include(x => x.Contacts)
            .FirstOrDefault(x => x.ExternalId == externalId);
        }

        public Tasker GetTaskerById(int id)
        {
           return _context.Taskers
            .Include(x => x.TaskerCerts)
            .Include(x => x.Contacts)
            .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TaskerCert> GetAllTaskerCertsByTaskerId(int taskerId)
        {
            return _context.TaskerCerts
            .Include(x=> x.Tasker)
            .Include(x=> x.Category)
            .Where(x => x.TaskerId == taskerId);
        }
    }
}