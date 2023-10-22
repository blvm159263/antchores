using AuthService.Repositories.Entities;
using AuthService.Repositories.Models;
using AuthService.Repositories.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Services.Services.Impl
{
    public class TaskerService : ITaskerService
    {
        private readonly TaskerRepository _taskerRepository;
        private readonly IMapper _mapper;

        public TaskerService(TaskerRepository taskerRepository, IMapper mapper)
        {
            _taskerRepository = taskerRepository;
            _mapper = mapper;
        }

        public bool AccountExists(int accountId)
        {
            var res = _taskerRepository.AccountExists(accountId);
            return res;
        }

        public TaskerReadModel CreateTasker(int accountId, TaskerCreateModel taskerCreateModel)
        {
            var cusModel = _mapper.Map<Tasker>(taskerCreateModel);
            
            cusModel.AccountId = accountId;

            _taskerRepository.CreateTasker(cusModel);

            var taskerReadModel = _mapper.Map<TaskerReadModel>(cusModel);

            return taskerReadModel;
        }

        public IEnumerable<TaskerReadModel> GetAllTaskers()
        {
            var taskers = _taskerRepository.GetAll();

            var taskerReadModels = _mapper.Map<IEnumerable<TaskerReadModel>>(taskers);

            return taskerReadModels;
        }

        public TaskerReadModel GetTaskerById(int id)
        {
            var tasker = _taskerRepository.GetTaskerById(id);

            var taskerReadModel = _mapper.Map<TaskerReadModel>(tasker);

            return taskerReadModel;
        }
    }
}
