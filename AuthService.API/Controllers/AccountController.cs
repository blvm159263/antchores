using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AuthService.Repositories.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;
using AuthService.Services.CacheService;

namespace AuthService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private ICacheService _cacheService;


        public AccountController(IAccountRepository accountRepository, IMapper mapper, ICacheService cacheService)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AccountReadModel>> GetAllAccounts()
        {
            string key = "allAccounts";
            var cacheAccounts = _cacheService.GetData<IEnumerable<AccountReadModel>>("allAccounts");

            if(cacheAccounts == null)
            {
                var accounts = _accountRepository.GetAllAccounts();

                cacheAccounts = _mapper.Map<IEnumerable<AccountReadModel>>(accounts);

                _cacheService.SetData(key, cacheAccounts);

                return Ok(cacheAccounts);
            }

            return Ok(cacheAccounts);
        }

        [HttpGet("{id}", Name = "GetAccountById")]
        public ActionResult<AccountReadModel> GetAccountById(int id)
        {
            string key = $"account-{id}";
            var cacheAccount = _cacheService.GetData<AccountReadModel>(key);


            if(cacheAccount == null)
            {
                var account = _accountRepository.GetAccountById(id);
                cacheAccount = _mapper.Map<AccountReadModel>(account);

                _cacheService.SetData<AccountReadModel>(key, cacheAccount);
                return Ok(cacheAccount);
            }

            return Ok(cacheAccount);
        }

        [HttpPost]
        public  ActionResult<AccountReadModel> CreateAccount(AccountCreateModel accountCreateModel)
        {
            if(_accountRepository.PhoneNumberExits(accountCreateModel.PhoneNumber))
                return BadRequest("Phone number already exists");
            
            var accModel = _mapper.Map<Account>(accountCreateModel);
            accModel.Status = true;
            accModel.UpdatedAt = System.DateTime.Now;
            accModel.CreatedAt = System.DateTime.Now;
            accModel.Role = Role.Customer;
            accModel.Balance = 0;
            _accountRepository.CreateAccount(accModel);

            var cusRead = _mapper.Map<AccountReadModel>(accModel);

            return CreatedAtRoute(nameof(GetAccountById), new { Id = accModel.Id }, accModel);
        }
    }

}