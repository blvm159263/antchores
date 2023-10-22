using AuthService.Repositories.Entities;
using AuthService.Repositories.Enums;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AuthService.Services.CacheService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services.Impl
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(AccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public AccountReadModel CreateAccount(AccountCreateModel accountCreateModel)
        {
            var accModel = _mapper.Map<Account>(accountCreateModel);
            accModel.Status = true;
            accModel.UpdatedAt = System.DateTime.Now;
            accModel.CreatedAt = System.DateTime.Now;
            accModel.Role = Role.Customer;
            accModel.Balance = 0;
            _accountRepository.CreateAccount(accModel);

            var accountModel = _mapper.Map<AccountReadModel>(accModel);
            return accountModel;
        }

        public AccountReadModel GetAccountById(int id)
        {
            var account = _accountRepository.GetAccountById(id);
            var accountModel = _mapper.Map<AccountReadModel>(account);
            return accountModel;
        }

        public IEnumerable<AccountReadModel> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountModels = _mapper.Map<IEnumerable<AccountReadModel>>(accounts);
            return accountModels;
        }

        public bool PhoneNumberExits(string phoneNumber)
        {
            return _accountRepository.PhoneNumberExits(phoneNumber);
        }
    }
}
