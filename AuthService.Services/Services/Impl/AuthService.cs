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
    public class AuthService : IAuthService
    {
        private readonly AccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountReadModel Authenticate(string phoneNumber, string password)
        {
            throw new NotImplementedException();
        }
    }
}
