using AuthService.Models;

namespace AuthService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCustomer(CustomerPublishedModel customerPublishedModel);

        void PublishNewTasker(TaskerPublishedModel taskerPublishedModel);
    }
}