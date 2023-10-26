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
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryReadModel>().ReverseMap();
            CreateMap<Category, CategoryCreateModel>().ReverseMap();
        }
    }
}
