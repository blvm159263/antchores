using System;
using System.Collections.Generic;
using ProductService.Repositories.Entities;
using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using AuthService;

namespace ProductService.Services.SyncDataServices.Grpc
{
    public class CustomerDataClient : ICustomerDataClient
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public CustomerDataClient(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }
        public IEnumerable<Customer> ReturnAllCustomers()
        {
            Console.WriteLine($"--> Calling GRPC Service {_config["GrpcAuth"]}");
            var channel =GrpcChannel.ForAddress(_config["GrpcAuth"]);
            var client = new GrpcCustomer.GrpcCustomerClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllCustomers(request);
                return _mapper.Map<IEnumerable<Customer>>(reply.Customer);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Tasker> ReturnAllTaskers()
        {
            Console.WriteLine($"--> Calling GRPC Service {_config["GrpcAuth"]}");
            var channel =GrpcChannel.ForAddress(_config["GrpcAuth"]);
            var client = new GrpcCustomer.GrpcCustomerClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllTaskers(request);
                return _mapper.Map<IEnumerable<Tasker>>(reply.Tasker);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}