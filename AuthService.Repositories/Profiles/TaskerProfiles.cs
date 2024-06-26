using AutoMapper;
using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;

namespace AuthService.Repositories.Profiles
{
    public class TaskerProfile : Profile
    {
        public TaskerProfile()
        {
            CreateMap<Tasker, TaskerReadModel>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Account.Balance));

            CreateMap<TaskerCreateModel, Tasker>();

            CreateMap<TaskerReadModel, TaskerPublishedModel>();

            CreateMap<Tasker, GrpcTaskerModel>()
                .ForMember(dest => dest.TaskerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}