using System.Threading.Tasks;
using AuthService.Models;

namespace AuthService.SyncDataServices.Http
{
    public interface IAuthDataClient
    {
        Task SendCustomerToAuth(CustomerReadModel customer);
    }
}