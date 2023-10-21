using AutoMapper;
using AuthService.BusinessObjects.Entities;
using AuthService.BusinessObjects.Models;

namespace AuthService.BusinessObjects.Profiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            CreateMap<Account, AccountReadModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<AccountCreateModel, Account>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Tasker, opt => opt.Ignore());
        }
    }
}