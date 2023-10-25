using System.Collections.Generic;
using AuthService.Repositories.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.Services.CacheService;
using AuthService.Services.Services;
using System.Linq;

namespace AuthService.API.Controllers
{
    [Route("api/a/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private ICacheService _cacheService;

        public AccountController(IAccountService accountService, ICacheService cacheService)
        {
            _accountService = accountService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AccountReadModel>> GetAllAccounts()
        {
            string key = "allAccounts";

            var cacheAccounts = _cacheService.GetData<IEnumerable<AccountReadModel>>("allAccounts");

            if(cacheAccounts == null)
            {
                cacheAccounts = _accountService.GetAllAccounts();

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
                cacheAccount = _accountService.GetAccountById(id);

                _cacheService.SetData(key, cacheAccount);

                return Ok(cacheAccount);
            }

            return Ok(cacheAccount);
        }

        [HttpPost]
        public  ActionResult<AccountReadModel> CreateAccount(AccountCreateModel accountCreateModel)
        {
            if(_accountService.PhoneNumberExits(accountCreateModel.PhoneNumber))
                return BadRequest("Phone number already exists");

            var cusRead = _accountService.CreateAccount(accountCreateModel);

            return CreatedAtRoute(nameof(GetAccountById), new { Id = cusRead.Id }, cusRead);
        }

    }

}