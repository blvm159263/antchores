using System.Collections.Generic;
using AuthService.Repositories.Entities;

namespace AuthService.Repositories.Repositories
{
    public interface ITaskerRepository
    {
        IEnumerable<Tasker> GetAll();

        Tasker GetTaskerById(int id);

        void CreateTasker(Tasker cus);

        bool AccountExists(int id);
    }
}