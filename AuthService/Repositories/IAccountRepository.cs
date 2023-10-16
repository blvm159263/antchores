using System.Collections.Generic;
using AuthService.Entities;

namespace AuthService.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();

        Account GetAccountById(int id);

        void CreateAccount(Account acc);

        bool PhoneNumberExits(string phoneNumber);
    }
}