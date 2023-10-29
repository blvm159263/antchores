using AuthService.Repositories.Data;
using AuthService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Repositories
{
    public class TaskerRepository
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
            if (tasker == null)
                throw new ArgumentNullException(nameof(tasker));

            _context.Taskers.Add(tasker);
            _context.SaveChanges();
        }

        public IEnumerable<Tasker> GetAll()
        {
            return _context.Taskers
                .Include(x => x.Account)
                .ToList();
        }

        public Tasker GetTaskerById(int id)
        {
            return _context.Taskers
                .Include(x => x.Account)
                .FirstOrDefault(c => c.Id == id);
        }

        public Tasker GetTaskerByAccountId(int accountId)
        {
            return _context.Taskers
                .Include(x => x.Account)
                .FirstOrDefault(c => c.AccountId == accountId);
        }
    }
}
