using AuthService.BusinessObjects.Models;

namespace AuthService.DataAccess.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewCustomer(CustomerPublishedModel customerPublishedModel);

        void PublishNewTasker(TaskerPublishedModel taskerPublishedModel);
    }
}