using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using ProductService.Repositories.Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Services.Services.Impl
{
    public class TaskerService : ITaskerService
    {
        private readonly ITaskerRepository _taskerRepository;
        private readonly IMapper _mapper;
        private readonly IContractRepository _contractRepository;
        private readonly ITaskerCertRepository _taskerCertRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskerService(ITaskerRepository taskerRepository, IContractRepository contactRepository, IMapper mapper, ITaskerCertRepository taskerCertRepository)
        {
            _taskerRepository = taskerRepository;
            _mapper = mapper;
            _contractRepository = contactRepository;
            _taskerCertRepository = taskerCertRepository;
        }

        public IEnumerable<TaskerCertReadModel> GetTaskerCertsByTaskerId(int taskerId)
        {
            IEnumerable<TaskerCert> taskerCerts = _taskerRepository.GetAllTaskerCertsByTaskerId(taskerId);
            return _mapper.Map<IEnumerable<TaskerCertReadModel>>(taskerCerts);
        }
        public IEnumerable<OrderReadModel> GetOrdersAvailableOfTasker(int taskerId, DateTime time)
        {
            IEnumerable<Contract> contacts = _contractRepository.GetContractsByTaskerId(taskerId);
            DateTime endDay = time.AddDays(1);
            endDay = new DateTime(endDay.Year, endDay.Month, endDay.Day, 0, 0, 0);
            time = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            var availableOrders = contacts
                .Where(contact => contact.Order.StartTime > time && contact.Order.StartTime < endDay)
                .Select(contact => _mapper.Map<OrderReadModel>(contact.Order));

            return availableOrders;
        }

        public TaskerModel AddCategoryServiceForTasker(int taskerId, List<int> categoryIds)
        {
            TaskerModel taskerModel = new TaskerModel();

            foreach (int categoryId in categoryIds)
            {
                TaskerCert taskerCert = new TaskerCert
                {
                    TaskerId = taskerId,
                    CategoryId = categoryId,
                    CreateAt = DateTime.Now,
                    Status = true
                };

                _taskerCertRepository.AddCategoryForTasker(taskerCert);

                Tasker tasker = _taskerRepository.GetTaskerById(taskerId);

                IEnumerable<TaskerCert> taskerCerts = tasker.TaskerCerts;

                IEnumerable<Category> categories = taskerCerts.Select(cert => cert.Category);

                IEnumerable<CategoryReadModel> categoryReadModels = _mapper.Map<IEnumerable<CategoryReadModel>>(categories);

                taskerModel = _mapper.Map<TaskerModel>(tasker);

                taskerModel.Categories = categoryReadModels;
            }

            return taskerModel;
        }

        public TaskerModel GetTaskerById(int taskerId)
        {
            Tasker tasker = _taskerRepository.GetTaskerById(taskerId);

            IEnumerable<TaskerCert> taskerCerts = tasker.TaskerCerts;

            IEnumerable<Category> categories = taskerCerts.Select(cert => cert.Category);

            IEnumerable<CategoryReadModel> categoryReadModels = _mapper.Map<IEnumerable<CategoryReadModel>>(categories);

            TaskerModel taskerModel = _mapper.Map<TaskerModel>(tasker);

            taskerModel.Categories = categoryReadModels;

            return taskerModel;
        }

        public bool CreateContract(ContractCreateModel contractCreateModel)
        {
            var contract = _mapper.Map<Contract>(contractCreateModel);
            return _contractRepository.CreateContact(contract);
        }

        public bool IsContractExist(ContractCreateModel contractCreateModel)
        {
            var contact = _contractRepository.GetContractsByTaskerIdAndOrderId(contractCreateModel.TaskerId, contractCreateModel.OrderId);
            if(contact == null)
            {
                return false;
            }
            return true;
        }
    }
}