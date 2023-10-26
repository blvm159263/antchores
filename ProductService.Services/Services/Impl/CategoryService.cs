using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public IEnumerable<CategoryReadModel> GetAllCategories()
        {
            var categories = _categoryRepository.GetCategories();
            return _mapper.Map<IEnumerable<CategoryReadModel>>(categories);
        }

        public CategoryReadModel GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryReadModel>(category);
        }

        public CategoryReadModel AddCategory(CategoryCreateModel categoryCreateModel)
        {
            var category = _mapper.Map<Category>(categoryCreateModel);
            category.Status = true;
            _categoryRepository.AddCategory(category);

            return _mapper.Map<CategoryReadModel>(category);
        }

        public CategoryReadModel UpdateCategory(CategoryCreateModel categoryCreateModel, int id)
        {
            var category = _mapper.Map<Category>(categoryCreateModel);
            category.Id = id;
            _categoryRepository.UpdateCategory(category);

            return _mapper.Map<CategoryReadModel>(category);
        }

        public CategoryReadModel DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            category.Status = false;
            _categoryRepository.UpdateCategory(category);

            return _mapper.Map<CategoryReadModel>(category);
        }
    }
}
