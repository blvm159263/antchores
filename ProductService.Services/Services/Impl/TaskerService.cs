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
        private readonly IContactRepository _contactRepository;

        public TaskerService(ITaskerRepository taskerRepository, IContactRepository contactRepository, IMapper mapper)
        {
            _taskerRepository = taskerRepository;
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        public IEnumerable<TaskerCertReadModel> GetTaskerCertsByTaskerId(int taskerId)
        {
            IEnumerable<TaskerCert> taskerCerts = _taskerRepository.GetAllTaskerCertsByTaskerId(taskerId);
            return _mapper.Map<IEnumerable<TaskerCertReadModel>>(taskerCerts);
        }
        public IEnumerable<OrderReadModel> GetOrdersAvailableOfTasker(int taskerId, DateTime time)
        {
            IEnumerable<Contact> contacts = _contactRepository.GetContactsByTaskerId(taskerId);
            Console.WriteLine(time);
            Console.WriteLine("contact: " + contacts.Count());
            var availableOrders = contacts
                .Where(contact => contact.Order.StartTime > time)
                .Select(contact => _mapper.Map<OrderReadModel>(contact.Order));

            return availableOrders;
        }
    }
}