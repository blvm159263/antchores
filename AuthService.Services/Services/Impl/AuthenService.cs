using AuthService.Repositories.Entities;
using AuthService.Repositories.Enums;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using System;

namespace AuthService.Services.Services.Impl
{
    public class AuthenService : IAuthenService
    {
        private readonly AccountRepository _accountRepository;
        private readonly ITokenService _tokenService;

        public AuthenService(AccountRepository accountRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        public AuthenticateResponseModel Authenticate(string phoneNumber, string password)
        {
            var account = _accountRepository.GetAccountByPhoneNumberAndPassword(phoneNumber, password);

            if (account == null)
            {
                return null;
            }

            AccountModel accountModel = new AccountModel
            {
                AccountId = account.Id,
                PhoneNumber = account.PhoneNumber,
                Role = account.Role.ToString()
            };

            var token = _tokenService.GenerateToken(accountModel);

            return new AuthenticateResponseModel
            {
                Account = accountModel,
                Token = token
            };
        }

        public AuthenticateResponseModel Register(AuthenticateRequestModel registerRequestModel)
        {
            var account = _accountRepository.GetAccountByPhoneNumberAndPassword(registerRequestModel.PhoneNumber, registerRequestModel.PhoneNumber);

            if (account != null)
            {
                return null;
            }

            account = new Account
            {
                PhoneNumber = registerRequestModel.PhoneNumber,
                Password = registerRequestModel.Password,
                Role = Role.Customer
            };

            _accountRepository.CreateAccount(account);

            AccountModel accountModel = new AccountModel
            {
                AccountId = account.Id,
                PhoneNumber = account.PhoneNumber,
                Role = account.Role.ToString()
            };

            var token = _tokenService.GenerateToken(accountModel);

            return new AuthenticateResponseModel
            {
                Account = accountModel,
                Token = token
            };
        }

        public AuthenticateResponseModel RegisterAsCustomer(AuthRequestCustomerModel authRequestCustomerModel)
        {
            throw new NotImplementedException();
        }

        public AuthenticateResponseModel RegisterAsTasker(AuthRequestTaskerModel authRequestTaskerModel)
        {
            throw new NotImplementedException();
        }
    }
}
