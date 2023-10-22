using System.Threading.Tasks;
using AuthService.Repositories.Models;

namespace AuthService.Services.SyncDataServices.Http
{
    public interface IAuthDataClient
    {
        Task SendCustomerToAuth(CustomerReadModel customer);
    }
}