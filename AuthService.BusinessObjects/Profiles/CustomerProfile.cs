using AutoMapper;
using AuthService.BusinessObjects.Entities;
using AuthService.BusinessObjects.Models;

namespace AuthService.BusinessObjects.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerReadModel>();
            CreateMap<CustomerCreateModel, Customer>();
            CreateMap<CustomerReadModel, CustomerPublishedModel>();
            CreateMap<Customer, GrpcCustomerModel>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}