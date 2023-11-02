using AuthService.Repositories.Enums;
using AuthService.Repositories.Models;
using AuthService.Services.AsyncDataServices;
using AuthService.Services.Services;
using AuthService.Services.SyncDataServices.Http;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthService.API.Controllers
{
    [Route("api/a/auths")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenService _authenService;
        private readonly IAccountService _accountService;
        private readonly ITaskerService _taskerService;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ICustomerService _customerService;

        public AuthenticationController(
            IAuthenService authenService, 
            IAccountService accountService,
            ITaskerService taskerService,
            IMapper mapper,
            IAuthDataClient authDataClient,
            IMessageBusClient messageBusClient,
            ICustomerService customerService
            )
        {
            _authenService = authenService;
            _accountService = accountService;
            _taskerService = taskerService;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
            _customerService = customerService;
        }

        [HttpPost("login")]
        public ActionResult Authenticate([FromBody] AuthenticateRequestModel authenticateRequestModel)
        {
            var account = 
                _authenService.Authenticate(authenticateRequestModel.PhoneNumber, authenticateRequestModel.Password);
            
            if (account == null)
            {
                return NotFound(new { message = "Phone number or password is incorrect" });
            }

            return Ok(account);
        }

        /*[HttpPost("register")]
        public ActionResult Register(AuthenticateRequestModel authenticateRequestModel)
        {
            var res = _authenService.Register(authenticateRequestModel);

            if (res == null)
            {
                return BadRequest(new { message = "Phone number is already taken" });
            }

            return Ok(res);
        }*/

        [HttpPost("register/customer")]
        public async Task<ActionResult> RegisterAsCustomer(AuthRequestCustomerModel auth)
        {
            AccountCreateModel accountCreateModel = new AccountCreateModel
            {
                PhoneNumber = auth.PhoneNumber,
                Password = auth.Password
            };

            var res = _accountService.CreateAccount(accountCreateModel, Role.Customer);

            if (res == null)
            {
                return BadRequest(new { message = "Phone number is already taken" });
            }

            if (!_customerService.AccountExists(res.Id))
                return NotFound();

            var customerCreateModel = new CustomerCreateModel
            {
                Name = auth.Name,
                Email = auth.Email,
                Address = auth.Address,
                Status = true
            };

            var cusRead = _customerService.CreateCustomer(res.Id, customerCreateModel);

            //Send Sync Message
            try
            {
                await _authDataClient.SendCustomerToAuth(cusRead);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send synchronously!: " + ex.Message);
            }

            //Send Async Message
            try
            {
                var CustomerPublishedModel = _mapper.Map<CustomerPublishedModel>(cusRead);
                CustomerPublishedModel.Event = "Customer_Published";
                _messageBusClient.PublishNewCustomer(CustomerPublishedModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }
            return Created("", cusRead);
        }

        [HttpPost("register/tasker")]
        public async Task<ActionResult> RegisterAsTasker(AuthRequestTaskerModel auth)
        {
            AccountCreateModel accountCreateModel = new AccountCreateModel
            {
                PhoneNumber = auth.PhoneNumber,
                Password = auth.Password
            };

            var res = _accountService.CreateAccount(accountCreateModel, Role.Tasker);

            if (res == null)
            {
                return BadRequest(new { message = "Phone number is already taken" });
            }

            if (!_taskerService.AccountExists(res.Id))
                return NotFound();

            var taskerCreateModel = new TaskerCreateModel
            {
                Name = auth.Name,
                Email = auth.Email,
                Address = auth.Address,
                Identification = auth.Identification,
                Status = true
            };

            var cusRead = _taskerService.CreateTasker(res.Id, taskerCreateModel);

            //Send Async Message
            try
            {
                var taskerPublishedModel = _mapper.Map<TaskerPublishedModel>(cusRead);
                taskerPublishedModel.Event = "Tasker_Published";
                _messageBusClient.PublishNewTasker(taskerPublishedModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }
            return Created("", cusRead);
        }
    }
}
