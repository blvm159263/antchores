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
                UserId = account.Id,
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
    }
}
