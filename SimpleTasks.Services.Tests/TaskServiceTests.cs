using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTasks.Data.Models;
using SimpleTasks.Data.Repositories;
using Moq;
using System.Linq;
using System.Collections.Generic;
using SimpleTasks.Services.Models;
using AutoMapper;
using SimpleTasks.Services.MappingProfiles;

namespace SimpleTasks.Services.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        private IList<Task> _tasks;
        private Mock<IRepository<Task>> _taskRepository;
        private TaskService _taskService;

        [TestInitialize]
        public void SetupDataForTests()
        {
            _tasks = new List<Task>();
            _taskRepository = new Mock<IRepository<Task>>();
            _taskService = new TaskService(_taskRepository.Object);

            _taskRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns((int taskId) => _tasks.FirstOrDefault(t => t.Id == taskId));

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new TaskProfile());
            });
        }

        [TestMethod]
        public void WhenGettingAListOfTasks_ExpectTasksToBeReturned()
        {
            // Arrage
            _tasks = new List<Task> { new Task() };
            _taskRepository.Setup(r => r.List()).Returns(_tasks);

            // Act
            var tasks = _taskService.GetAll();

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count());
        }

        [TestMethod]
        public void WhenGettingAListOfTasks_ExpectTasksToBeReturnedInDueDateOrder()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "1", DueDate = DateTime.Now, CompletedDateTime = DateTime.Now }, 
                new Task { Id = 2, Title = "2", DueDate = DateTime.Today }
            };
            _taskRepository.Setup(r => r.List()).Returns(_tasks);

            // Act
            var tasks = _taskService.GetAll();

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
            Assert.AreEqual("2", tasks.ElementAt(0).Title);
            Assert.AreEqual("1", tasks.ElementAt(1).Title);
        }

        [TestMethod]
        public void WhenGettingAListOfOutstandingTasks_ExpectOnlyOutstansdingTasksToBeReturned()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Title = "1", DueDate = DateTime.Now, CompletedDateTime = DateTime.Now }, 
                new Task { Title = "2", DueDate = DateTime.Now }
            };
            _taskRepository.Setup(r => r.List()).Returns(_tasks);

            // Act
            var tasks = _taskService.GetAllOutstanding();

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(1, tasks.Count());
            Assert.AreEqual("2", tasks.ElementAt(0).Title);
        }

        [TestMethod]
        public void WhenGettingAListOfOutstandingTasks_ExpectTasksToBeReturnedInDueDateOrder()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "1", DueDate = DateTime.Now }, 
                new Task { Id = 2, Title = "2", DueDate = DateTime.Today }
            };
            _taskRepository.Setup(r => r.List()).Returns(_tasks);

            // Act
            var tasks = _taskService.GetAllOutstanding();

            // Assert
            Assert.IsNotNull(tasks);
            Assert.AreEqual(2, tasks.Count());
            Assert.AreEqual("2", tasks.ElementAt(0).Title);
            Assert.AreEqual("1", tasks.ElementAt(1).Title);
        }

        [TestMethod]
        public void WhenGettingATaskById_ExpectTheCorrectTaskToBeReturned()
        {
            // Arrage
            var id = 1;
            _tasks = new List<Task> { 
                new Task { Id = id, Title = "1", DueDate = DateTime.Now, CompletedDateTime = DateTime.Now }, 
                new Task { Id = 2, Title = "2", DueDate = DateTime.Now }
            };
            _taskRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns((int taskId) => _tasks.FirstOrDefault(t => t.Id == taskId));

            // Act
            var task = _taskService.GetById(id);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(id, task.Id);
        }

        [TestMethod]
        public void WhenGettingATasksByIdThatDoesntExist_ExpectNull()
        {
            // Arrage
            var id = 3;
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "1", DueDate = DateTime.Now, CompletedDateTime = DateTime.Now }, 
                new Task { Id = 2, Title = "2", DueDate = DateTime.Now }
            };
            
            _taskRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns((int taskId) => _tasks.FirstOrDefault(t => t.Id == taskId));

            // Act
            var task = _taskService.GetById(id);

            // Assert
            Assert.IsNull(task);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenCreatingANewTaskPassingNull_ExpectException()
        {
            // Arrage

            // Act
            var success = _taskService.CreateTask(null);

            // Assert
            // Exception
        }

        [TestMethod]
        public void WhenCreatingANewTask_ExpectSuccess()
        {
            // Arrage
            var task = new TaskModel
            {
                Id = 1,
                Title = "A new task",
                DueDate = DateTime.Today.AddDays(1)
            };
            _taskRepository.Setup(r => r.Insert(It.IsAny<Task>()))
                .Returns((Task inserted) => inserted);

            // Act
            var newTask = _taskService.CreateTask(task);

            // Assert
            Assert.IsNotNull(newTask);
            Assert.AreEqual(task.Id, newTask.Id);
            Assert.AreEqual(task.Title, newTask.Title);
            Assert.AreEqual(task.DueDate, newTask.DueDate);
        }

        [TestMethod]
        public void WhenCreatingANewTaskWithoutId_ExpectSuccess()
        {
            // Arrage
            var task = new TaskModel
            {
                Title = "A new task",
                DueDate = DateTime.Today.AddDays(1)
            };
            _taskRepository.Setup(r => r.Insert(It.IsAny<Task>()))
                .Returns((Task inserted) => 
                    { 
                        inserted.Id = 1;
                        return inserted;
                    });

            // Act
            var newTask = _taskService.CreateTask(task);

            // Assert
            Assert.IsNotNull(newTask);
            Assert.AreNotEqual(0, newTask.Id);
            Assert.AreEqual(1, newTask.Id);
            Assert.AreEqual("A new task", newTask.Title);
            Assert.AreEqual(DateTime.Today.AddDays(1), newTask.DueDate);
        }

        [TestMethod]
        public void WhenCreatingANewTaskWithSameId_ExpectFailure()
        {
            // Arrage
            var id = 1;
            _tasks = new List<Task> { 
                new Task { Id = id, Title = "1", DueDate = DateTime.Now }
            };
            var task = new TaskModel
            {
                Id = id,
                Title = "A new task",
                DueDate = DateTime.Today.AddDays(1)
            };
            _taskRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns((int taskId) => _tasks.FirstOrDefault(t => t.Id == taskId));

            // Act
            var newTask = _taskService.CreateTask(task);

            // Assert
            Assert.IsNull(newTask);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenEditingATaskPassingNull_ExpectException()
        {
            // Arrage

            // Act
            var success = _taskService.EditTask(null);

            // Assert
            // Exception
        }

        [TestMethod]
        public void WhenEditingATaskThatDoesntExist_ExpectFailure()
        {
            // Arrage
            var task = new TaskModel
            {
                Id = 1,
                Title = "An edited task",
                DueDate = DateTime.Today.AddDays(1)
            };

            // Act
            var editedTask = _taskService.EditTask(task);

            // Assert
            Assert.IsNull(editedTask);
        }
        [TestMethod]
        public void WhenEditingATask_ExpectSuccess()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "A new task", DueDate = DateTime.Now }
            };
            var task = new TaskModel
            {
                Id = 1,
                Title = "An edited task",
                DueDate = DateTime.Today.AddDays(1)
            };

            // Act
            var editedTask = _taskService.EditTask(task);

            // Assert
            Assert.IsNotNull(editedTask);
            Assert.AreEqual(1, editedTask.Id);
            Assert.AreEqual("An edited task", editedTask.Title);
            Assert.AreEqual(DateTime.Today.AddDays(1), editedTask.DueDate);
        }

        [TestMethod]
        public void WhenEditingAnOutstandingTask_SettingToCompleted_ExpectSuccess()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "A new task", DueDate = DateTime.Now }
            };
            var task = new TaskModel
            {
                Id = 1,
                Title = "An edited task",
                DueDate = DateTime.Today.AddDays(1),
                IsComplete = true
            };

            // Act
            var editedTask = _taskService.EditTask(task);

            // Assert
            Assert.IsNotNull(editedTask);
            Assert.AreEqual(1, editedTask.Id);
            Assert.AreEqual("An edited task", editedTask.Title);
            Assert.AreEqual(DateTime.Today.AddDays(1), editedTask.DueDate);
            Assert.AreEqual(true, editedTask.IsComplete);
            Assert.AreEqual(true, editedTask.CompletedDateTime.HasValue);
        }

        [TestMethod]
        public void WhenEditingATaskCompletedTask_SettingToNotCompleted_ExpectSuccess()
        {
            // Arrage
            _tasks = new List<Task> { 
                new Task { Id = 1, Title = "A new task", DueDate = DateTime.Now, CompletedDateTime = DateTime.Now }
            };
            var task = new TaskModel
            {
                Id = 1,
                Title = "An edited task",
                DueDate = DateTime.Today.AddDays(1),
                IsComplete = false
            };

            // Act
            var editedTask = _taskService.EditTask(task);

            // Assert
            Assert.IsNotNull(editedTask);
            Assert.AreEqual(1, editedTask.Id);
            Assert.AreEqual("An edited task", editedTask.Title);
            Assert.AreEqual(DateTime.Today.AddDays(1), editedTask.DueDate);
            Assert.AreEqual(false, editedTask.IsComplete);
            Assert.AreEqual(false, editedTask.CompletedDateTime.HasValue);
        }

        [TestMethod]
        public void WhenCompletingATask_ExpectSuccess()
        {
            // Arrage
            var task = new Task
            {
                Id = 1,
                Title = "An outstanding task",
                DueDate = DateTime.Today.AddDays(1)
            };
            _tasks = new List<Task> { 
                task 
            };

            // Act
            var completedTask = _taskService.CompleteTask(1);

            // Assert
            Assert.IsNotNull(completedTask);
            Assert.AreEqual(1, completedTask.Id);
            Assert.AreEqual(true, completedTask.IsComplete);
        }

        [TestMethod]
        public void WhenCompletingATaskThatDoesntExist_ExpectFailure()
        {
            // Arrage

            // Act
            var completedTask = _taskService.CompleteTask(1);

            // Assert
            Assert.IsNull(completedTask);
        }
    }
}
