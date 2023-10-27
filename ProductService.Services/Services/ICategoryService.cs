using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services
{
    public interface ICategoryService
    {
        IEnumerable<CategoryReadModel> GetAllCategories(); 
        CategoryReadModel GetCategoryById(int id);
        CategoryReadModel AddCategory(CategoryCreateModel categoryCreateModel);
        CategoryReadModel UpdateCategory(CategoryCreateModel categoryCreateModel, int id);
        CategoryReadModel DeleteCategory(int id);
    }
}
