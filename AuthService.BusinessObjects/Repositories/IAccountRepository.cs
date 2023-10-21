using System.Collections.Generic;
using AuthService.BusinessObjects.Entities;

namespace AuthService.BusinessObjects.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();

        Account GetAccountById(int id);

        void CreateAccount(Account acc);

        bool PhoneNumberExits(string phoneNumber);
    }
}