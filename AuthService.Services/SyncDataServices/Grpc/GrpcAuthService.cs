using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using AuthService.Repositories.Repositories;

namespace AuthService.Services.SyncDataServices.Grpc
{
    public class GrpcAuthService : GrpcCustomer.GrpcCustomerBase
    {
        private readonly CustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly TaskerRepository _taskerRepository;

        public GrpcAuthService(CustomerRepository repository, TaskerRepository taskerRepository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _taskerRepository = taskerRepository;
        }

        public override Task<CustomerResponse> GetAllCustomers(GetAllRequest request, ServerCallContext context)
        {
            var response = new CustomerResponse();
            var customers = _repository.GetAll();

            foreach (var cate in customers)
            {
                response.Customer.Add(_mapper.Map<GrpcCustomerModel>(cate));
            }

            return Task.FromResult(response);
        }

        public override Task<TaskerResponse> GetAllTaskers(GetAllRequest request, ServerCallContext context)
        {
            var response = new TaskerResponse();
            var taskers = _taskerRepository.GetAll();

            foreach (var cate in taskers)
            {
                response.Tasker.Add(_mapper.Map<GrpcTaskerModel>(cate));
            }

            return Task.FromResult(response);
        }

    }
}