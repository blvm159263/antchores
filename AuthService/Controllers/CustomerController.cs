using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AuthService.AsyncDataServices;
using AuthService.Entities;
using AuthService.Models;
using AuthService.Repositories;
using AuthService.SyncDataServices.Http;

namespace AuthService.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAuthDataClient _authDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public CustomerController(
                ICustomerRepository customerRepository,
                IMapper mapper,
                IAuthDataClient authDataClient,
                IMessageBusClient messageBusClient)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _authDataClient = authDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerReadModel>> GetAllCustomers()
        {
            var customers = _customerRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<CustomerReadModel>>(customers));
        }


        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<IEnumerable<CustomerReadModel>> GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return Ok(_mapper.Map<CustomerReadModel>(customer));
        }

        [HttpPost("accounts/{accountId}")]
        public async Task<ActionResult<CustomerReadModel>> CreateCustomer(int accountId, CustomerCreateModel customerCreateModel)
        {

            if(!_customerRepository.AccountExists(accountId))
                return NotFound();
                

            var cusModel = _mapper.Map<Customer>(customerCreateModel);
            cusModel.AccountId = accountId;
            _customerRepository.CreateCustomer(cusModel);

            var cusRead = _mapper.Map<CustomerReadModel>(cusModel);

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
            catch(Exception ex)
            {
                Console.WriteLine("Could not send asynchronously!: " + ex.Message);
            }

            return CreatedAtRoute(nameof(GetCustomerById), new { Id = cusRead.Id }, cusRead);
        }
    }
}