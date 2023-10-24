using AuthService;
using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Profiles
{
    public class TaskerProfile : Profile
    {
        public TaskerProfile() {
            //Tasker
            
            //Tasker
            CreateMap<Tasker, TaskerReadModel>();
            CreateMap<TaskerPublishedModel, Tasker>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GrpcTaskerModel, Tasker>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.TaskerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Identification, opt => opt.MapFrom(src => src.Identification))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Contacts, opt => opt.Ignore())
            .ForMember(dest => dest.TaskerCerts, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            //TaskerCert
            CreateMap<TaskerCert, TaskerCertReadModel>()
                .ForMember(dest => dest.TaskerName, opt => opt.MapFrom(src => src.Tasker.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
