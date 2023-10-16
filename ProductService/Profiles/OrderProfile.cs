using ProductService.Entities;
using ProductService.Models;
using AutoMapper;
using AuthService;

namespace ProductService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //Order
            CreateMap<OrderCreateModel, Order>();
            CreateMap<Order, OrderReadModel>();

            //Customer
            CreateMap<Customer, CustomerReadModel>();
            CreateMap<CustomerPublishedModel, Customer>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcCustomerModel, Customer>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Orders, opt => opt.Ignore());

            //Tasker
            CreateMap<Tasker, TaskerReadModel>();
            CreateMap<TaskerPublishedModel, Tasker>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcTaskerModel, Tasker>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.TaskerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Contacts, opt => opt.Ignore())
            .ForMember(dest => dest.TaskerCerts, opt => opt.Ignore());
        }

    }
}