using System.Collections.Generic;
using System.Threading.Tasks;
using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AuthService.Repositories.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountController(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AccountReadModel>> GetAllAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            return Ok(_mapper.Map<IEnumerable<AccountReadModel>>(accounts));
        }

        [HttpGet("{id}", Name = "GetAccountById")]
        public ActionResult<AccountReadModel> GetAccountById(int id)
        {
            var account = _accountRepository.GetAccountById(id);
            return Ok(_mapper.Map<AccountReadModel>(account));
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