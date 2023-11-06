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
        private readonly AccountRepository _accountRepository;

        public TaskerService(TaskerRepository taskerRepository,
                            AccountRepository accountRepository,
                            IMapper mapper)
        {
            _taskerRepository = taskerRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
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

        public TaskerReadModel GetTaskerByAccountId(int accountId)
        {
            var tasker = _taskerRepository.GetTaskerByAccountId(accountId);

            var taskerReadModel = _mapper.Map<TaskerReadModel>(tasker);

            return taskerReadModel;
        }

        public TaskerReadModel GetTaskerById(int id)
        {
            var tasker = _taskerRepository.GetTaskerById(id);

            var taskerReadModel = _mapper.Map<TaskerReadModel>(tasker);

            return taskerReadModel;
        }

        public bool UpdateTasker(int taskerId, AuthRequestTaskerModel model)
        {
            var tasker = _taskerRepository.GetTaskerById(taskerId);

            var account = _accountRepository.GetAccountById(tasker.AccountId);

            account.PhoneNumber = model.PhoneNumber;
            account.Password = model.Password;
            if (_accountRepository.UpdateAccount(account))
            {
                tasker.Identification = model.Identification;
                tasker.Address = model.Address;
                tasker.Name = model.Name;
                tasker.Email = model.Email;
                if(_taskerRepository.UpdateTasker(tasker))
                    return true;
            }

            return false;
        }
    }
}
