using System;
using System.Collections.Generic;
using System.Linq;
using AuthService.Data;
using AuthService.Entities;

namespace AuthService.Repositories.Impl
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateAccount(Account Account)
        {
            if(Account == null)
                throw new ArgumentNullException(nameof(Account));

            _context.Accounts.Add(Account);
            _context.SaveChanges();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public Account GetAccountById(int id)
        {
            return _context.Accounts.FirstOrDefault(c => c.Id == id);
        }

        public bool PhoneNumberExits(string phoneNumber)
        {
            return _context.Accounts.Any(a => a.PhoneNumber == phoneNumber);
        }
    }
}