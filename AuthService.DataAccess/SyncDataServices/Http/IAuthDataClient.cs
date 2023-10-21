using System.Threading.Tasks;
using AuthService.BusinessObjects.Models;

namespace AuthService.DataAccess.SyncDataServices.Http
{
    public interface IAuthDataClient
    {
        Task SendCustomerToAuth(CustomerReadModel customer);
    }
}