using AuthService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services
{
    public interface IAccountService
    {
        IEnumerable<AccountReadModel> GetAllAccounts();
        AccountReadModel GetAccountById(int id);
        AccountReadModel CreateAccount(AccountCreateModel accountCreateModel);
        bool PhoneNumberExits(string phoneNumber);
    }
}
