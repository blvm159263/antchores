using AutoMapper;
using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;

namespace AuthService.Repositories.Profiles
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