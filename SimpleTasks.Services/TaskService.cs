using AutoMapper;
using SimpleTasks.Data.Models;
using SimpleTasks.Data.Repositories;
using SimpleTasks.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTasks.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskModel> GetAll();
        IEnumerable<TaskModel> GetAllOutstanding();
        TaskModel GetById(int id);
        TaskModel CreateTask(TaskModel task);
        TaskModel EditTask(TaskModel task);
        TaskModel CompleteTask(int id);
    }

    public class TaskService : ITaskService
    {
        private readonly IRepository<Task> _taskRepository;

        public TaskService(IRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskModel> GetAll()
        {
            var tasks = _taskRepository.List().OrderBy(t => t.DueDate);
            return Mapper.Map<List<TaskModel>>(tasks);
        }

        public IEnumerable<TaskModel> GetAllOutstanding()
        {
            var tasks = _taskRepository.List().Where(t => !t.IsComplete()).OrderBy(t => t.DueDate);
            return Mapper.Map<List<TaskModel>>(tasks);
        }

        public TaskModel GetById(int id)
        {
            return Mapper.Map<TaskModel>(_taskRepository.GetById(id));
        }

        public TaskModel CreateTask(TaskModel task)
        {
            if (task == null) throw new ArgumentNullException("task");

            Task existingTask = null;

            if (task.Id != 0)
            {
                existingTask = _taskRepository.GetById(task.Id);
            }

            if (existingTask == null)
            {
                var newTask = Mapper.Map<Task>(task);
                return Mapper.Map<TaskModel>(_taskRepository.Insert(newTask));
            }

            return null;
        }

        public TaskModel EditTask(TaskModel task)
        {
            if (task == null) throw new ArgumentNullException("task");

            Task existingTask = _taskRepository.GetById(task.Id);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Details = task.Details;
                existingTask.DueDate = task.DueDate;
                existingTask.Owner = task.Owner;

                if (task.IsComplete && !existingTask.CompletedDateTime.HasValue)
                {
                    existingTask.CompletedDateTime = DateTime.Now.ToUniversalTime();
                }
                else if (!task.IsComplete && existingTask.CompletedDateTime.HasValue)
                {
                    existingTask.CompletedDateTime = null;
                }

                _taskRepository.Update(existingTask);

                return Mapper.Map<TaskModel>(existingTask);
            }

            return null;
        }

        public TaskModel CompleteTask(int id)
        {
            var task = _taskRepository.GetById(id);

            if (task != null)
            {
                task.CompletedDateTime = DateTime.Now.ToUniversalTime();

                _taskRepository.Update(task);

                return Mapper.Map<TaskModel>(task);
            }

            return null;
        }
    }
}
