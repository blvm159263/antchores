using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Data;
using AuthService.Entities;

namespace AuthService.Repositories.Impl
{
    public class TaskerRepository : ITaskerRepository
    {
        private readonly AppDbContext _context;

        public TaskerRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool AccountExists(int id)
        {
            return _context.Accounts.Any(a => a.Id == id);
        }

        public void CreateTasker(Tasker tasker)
        {
            if(tasker == null)
                throw new ArgumentNullException(nameof(tasker));

            _context.Taskers.Add(tasker);
            _context.SaveChanges();
        }

        public IEnumerable<Tasker> GetAll()
        {
            return _context.Taskers.ToList();
        }

        public Tasker GetTaskerById(int id)
        {
            return _context.Taskers.FirstOrDefault(c => c.Id == id);
        }
    }
}