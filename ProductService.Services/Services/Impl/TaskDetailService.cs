using AutoMapper;
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
    public class TaskDetailService : ITaskDetailService
    {
        private readonly ITaskDetailRepository _taskDetailRepository;
        private readonly IMapper _mapper;

        public TaskDetailService(ITaskDetailRepository taskDetailRepository, IMapper mapper)
        {
            _taskDetailRepository = taskDetailRepository;
            _mapper = mapper;
        }

        public TaskDetailReadModel CreateTaskDetail(TaskDetailCreateModel taskDetailCreateModel)
        {
            var taskDetail = _mapper.Map<TaskDetail>(taskDetailCreateModel);

            _taskDetailRepository.CreateTaskDetail(taskDetail);

            var taskDetailReadModel = _mapper.Map<TaskDetailReadModel>(taskDetail);

            return taskDetailReadModel;
        }

        public TaskDetailReadModel DeleteTaskDetail(int id)
        {
            var taskDetail = _taskDetailRepository.DeleteTaskDetail(id);

            var taskDetailReadModel = _mapper.Map<TaskDetailReadModel>(taskDetail);

            return taskDetailReadModel;
        }

        public IEnumerable<TaskDetailReadModel> GetAllTaskDetails()
        {
            var taskDetails = _taskDetailRepository.GetAllTaskDetails();

            var taskDetailReadModels = _mapper.Map<IEnumerable<TaskDetailReadModel>>(taskDetails);

            return taskDetailReadModels;
        }

        public TaskDetailReadModel GetTaskDetailById(int id)
        {
            var taskDetail = _taskDetailRepository.GetTaskDetailById(id);

            var taskDetailReadModel = _mapper.Map<TaskDetailReadModel>(taskDetail);

            return taskDetailReadModel;
        }

        public IEnumerable<TaskDetailReadModel> GetTaskDetailsByCategoryId(int id)
        {
            var taskDetail = _taskDetailRepository.GetTaskDetailsByCategoryId(id);

            var taskDetailReadModel = _mapper.Map<IEnumerable<TaskDetailReadModel>>(taskDetail);

            return taskDetailReadModel;
        }

        public TaskDetailReadModel UpdateTaskDetail(int id, TaskDetailCreateModel taskDetailCreateModel)
        {
            var taskDetail = _mapper.Map<TaskDetail>(taskDetailCreateModel);

            taskDetail.Id = id;

            _taskDetailRepository.UpdateTaskDetail(taskDetail);

            var taskDetailReadModel = _mapper.Map<TaskDetailReadModel>(taskDetail);

            return taskDetailReadModel;
        }
    }
}
