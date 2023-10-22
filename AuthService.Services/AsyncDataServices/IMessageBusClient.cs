using AuthService.Repositories.Models;

namespace AuthService.Services.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCustomer(CustomerPublishedModel customerPublishedModel);

        void PublishNewTasker(TaskerPublishedModel taskerPublishedModel);
    }
}