using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using AuthService.BusinessObjects.Repositories;

namespace AuthService.DataAccess.SyncDataServices.Grpc
{
    public class GrpcAuthService : GrpcCustomer.GrpcCustomerBase
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public GrpcAuthService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            var taskers = _repository.GetAll();

            foreach (var cate in taskers)
            {
                response.Tasker.Add(_mapper.Map<GrpcTaskerModel>(cate));
            }

            return Task.FromResult(response);
        }

    }
}