using AuthService.Repositories.Data;
using AuthService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Repositories.Repositories
{
    public class AccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateAccount(Account Account)
        {
            if (Account == null)
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
            return _context.Accounts.SingleOrDefault(c => c.Id == id);
        }

        public bool PhoneNumberExits(string phoneNumber)
        {
            return _context.Accounts.Any(c => c.PhoneNumber == phoneNumber);
        }


        public void UpdateAccount(Account Account)
        {
            if (Account == null)
                throw new ArgumentNullException(nameof(Account));

            _context.ChangeTracker.Clear();
            _context.Entry(Account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteAccount(Account Account)
        {
            if (Account == null)
                throw new ArgumentNullException(nameof(Account));

            _context.Accounts.Remove(Account);
            _context.SaveChanges();
        }

        public Account GetAccountByPhoneNumberAndPassword(string phoneNumber, string password)
        {
            return _context.Accounts.SingleOrDefault(c => c.PhoneNumber == phoneNumber && c.Password == password);
        }
    }
}
