using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AutoMapper;
using System.Collections.Generic;

namespace AuthService.Services.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(CustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public bool AccountExists(int accountId)
        {
            var res = _customerRepository.AccountExists(accountId);

            return res;
        }

        public CustomerReadModel CreateCustomer(int accountId, CustomerCreateModel customerCreateModel)
        {
            var cusModel = _mapper.Map<Customer>(customerCreateModel);

            cusModel.AccountId = accountId;
            
            _customerRepository.CreateCustomer(cusModel);

            var customerReadModel = _mapper.Map<CustomerReadModel>(cusModel);

            return customerReadModel;
        }

        public IEnumerable<CustomerReadModel> GetAllCustomers()
        {
            var customers = _customerRepository.GetAll();

            var customerModels = _mapper.Map<IEnumerable<CustomerReadModel>>(customers);

            return customerModels;
        }

        public CustomerReadModel GetCustomerById(int id)
        {
            var customer = _customerRepository.GetCustomerById(id);

            var customerModel = _mapper.Map<CustomerReadModel>(customer);

            return customerModel;
        }
    }
}
