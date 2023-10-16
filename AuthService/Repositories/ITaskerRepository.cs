using System.Collections.Generic;
using AuthService.Entities;

namespace AuthService.Repositories
{
    public interface ITaskerRepository
    {
        IEnumerable<Tasker> GetAll();

        Tasker GetTaskerById(int id);

        void CreateTasker(Tasker cus);

        bool AccountExists(int id);
    }
}