using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using AutoMapper;
using AuthService;
using System.Linq;

namespace ProductService.Repositories.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //Order
            CreateMap<OrderCreateModel, Order>();

            CreateMap<Order, OrderReadModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Customer.Address))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.OrderDetails.First().TaskDetail.Category.Name))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.GetEndTime()));

            //Customer
            CreateMap<Customer, CustomerReadModel>();
            
            CreateMap<CustomerPublishedModel, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<GrpcCustomerModel, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            //OrderDetail
            CreateMap<OrderDetail, OrderDetailReadModel>()
                .ForMember(dest => dest.TaskDetailName, opt => opt.MapFrom(x => x.TaskDetail.Name));

        }

    }
}