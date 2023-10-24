using System.Collections.Generic;
using ProductService.Repositories.Entities;

namespace ProductService.Repositories.Repositories
{
    public interface ITaskerRepository
    {
        //Tasker
        IEnumerable<Tasker> GetAllTaskers();
        Tasker GetTaskerById(int id);
        Tasker GetTaskerByExternalId(int externalId);
        void CreateTasker(Tasker tasker);

        //TaskerCert
        IEnumerable<TaskerCert> GetAllTaskerCertsByTaskerId(int taskerId);


    }
}