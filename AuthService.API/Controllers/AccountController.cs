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

namespace AuthService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private IDistributedCache _distributedCache;

        public AccountController(IAccountRepository accountRepository, IMapper mapper, IDistributedCache distributedCache)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AccountReadModel>> GetAllAccounts()
        {
            string? cacheAccounts = _distributedCache.GetString("allAccount");

            IEnumerable<AccountReadModel>? accountReads;

            if(string.IsNullOrEmpty(cacheAccounts))
            {
                var accounts = _accountRepository.GetAllAccounts();

                accountReads = _mapper.Map<IEnumerable<AccountReadModel>>(accounts);
                _distributedCache.SetString("allAccount", JsonSerializer.Serialize(accountReads));

                return Ok(accountReads);
            }

            accountReads = JsonSerializer.Deserialize<IEnumerable<AccountReadModel>>(cacheAccounts);

            return Ok(accountReads);
        }

        [HttpGet("{id}", Name = "GetAccountById")]
        public ActionResult<AccountReadModel> GetAccountById(int id)
        {
            string key = $"account-{id}";
            string? cacheAccount = _distributedCache.GetString(key);

            AccountReadModel? accountReadModel;

            if(string.IsNullOrEmpty(cacheAccount))
            {
                var account = _accountRepository.GetAccountById(id);
                accountReadModel = _mapper.Map<AccountReadModel>(account);

                _distributedCache.SetString(key, JsonSerializer.Serialize(accountReadModel));
                return Ok(accountReadModel);
            }

            accountReadModel = JsonSerializer.Deserialize<AccountReadModel>(cacheAccount);
            return Ok(accountReadModel);
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