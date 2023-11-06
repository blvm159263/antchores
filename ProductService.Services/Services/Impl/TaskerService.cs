using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Enums;
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
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;

        public TaskerService(ITaskerRepository taskerRepository,
                            IContractRepository contactRepository,
                            IMapper mapper,
                            ITaskerCertRepository taskerCertRepository,
                            IOrderRepository orderRepository)
        {
            _taskerRepository = taskerRepository;
            _mapper = mapper;
            _contractRepository = contactRepository;
            _taskerCertRepository = taskerCertRepository;
            _orderRepository = orderRepository;
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
                .Select(contact => _mapper.Map<OrderReadModel>(contact.Order))
                .OrderBy(o => o.StartTime);
            return availableOrders;
        }

        public IEnumerable<OrderReadModel> GetOrdersInProgressForTasker(int taskerId, DateTime nowTime)
        {
            IEnumerable<Contract> contacts = _contractRepository.GetContractsByTaskerId(taskerId);
            var availableOrders = contacts
                .Where(contact => contact.Order.StartTime < nowTime)
                .Select(contact => _mapper.Map<OrderReadModel>(contact.Order))
                .OrderBy(o => o.StartTime);

            var inProgress = availableOrders.Where(o => o.EndTime > nowTime);
            return inProgress;
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
            var contract = new Contract();
            contract.OrderId = contractCreateModel.OrderId;
            contract.TaskerId = contractCreateModel.TaskerId;
            contract.CreateAt = DateTime.Now;
            contract.Status = true;
            
            if (_contractRepository.CreateContact(contract))
            {
                var order = _orderRepository.GetOrderByOrderId(contractCreateModel.OrderId);
                order.State = OrderEnum.Accepted;
                return _orderRepository.UpdateOrder(order);
            }
            return false;
        }

        public bool IsContractExist(ContractCreateModel contractCreateModel)
        {
            var contact = _contractRepository.GetContractsByTaskerIdAndOrderId(contractCreateModel.TaskerId, contractCreateModel.OrderId);
            if (contact == null)
            {
                return false;
            }
            return true;
        }

        public bool DeleteContract(int taskerId, int orderId)
        {
            var order = _orderRepository.GetOrderByOrderId(orderId);
            if(order != null){
                order.State = OrderEnum.Pending;
                _orderRepository.UpdateOrder(order);
            }
            var contract = _contractRepository.GetContractsByTaskerIdAndOrderId(taskerId, orderId);
            if (contract == null)
            {
                return false;
            }
            return _contractRepository.DeleteContract(contract);
        }
    }
}