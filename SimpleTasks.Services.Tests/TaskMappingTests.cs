using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleTasks.Data.Models;
using SimpleTasks.Services.MappingProfiles;
using SimpleTasks.Services.Models;
using System;

namespace SimpleTasks.Services.Tests
{
    [TestClass]
    public class TaskMappingTests
    {
        [TestInitialize]
        public void SetupDataForTests()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new TaskProfile());
            });
        }

        [TestMethod]
        public void WhenMappingATaskToTaskModel_ExpectPropertiesToMatch()
        {
            // Arrage
            var task = new Task
            {
                Id = 1, 
                Title = "A task", 
                Details = "Task details", 
                DueDate = new DateTime(2014, 11, 1)
            };

            // Act
            var taskModel = Mapper.Map<TaskModel>(task);

            // Assert
            Assert.IsNotNull(taskModel);
            Assert.AreEqual(1, taskModel.Id);
            Assert.AreEqual("A task", taskModel.Title);
            Assert.AreEqual("Task details", taskModel.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), taskModel.DueDate);
            Assert.AreEqual(false, taskModel.IsComplete);
            Assert.AreEqual(TaskPriority.Normal, taskModel.PriorityLevel);
        }

        [TestMethod]
        public void WhenMappingACompletedTaskToTaskModel_ExpectPropertiesToMatch()
        {
            // Arrage
            var task = new Task
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                CompletedDateTime = DateTime.Now
            };

            // Act
            var taskModel = Mapper.Map<TaskModel>(task);

            // Assert
            Assert.IsNotNull(taskModel);
            Assert.AreEqual(1, taskModel.Id);
            Assert.AreEqual("A task", taskModel.Title);
            Assert.AreEqual("Task details", taskModel.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), taskModel.DueDate);
            Assert.AreEqual(true, taskModel.IsComplete);
        }

        [TestMethod]
        public void WhenMappingATaskModelToATask_ExpectPropertiesToMatch()
        {
            // Arrage
            var taskModel = new TaskModel
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                PriorityLevel = TaskPriority.Normal
            };

            // Act
            var task = Mapper.Map<Task>(taskModel);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.Id);
            Assert.AreEqual("A task", task.Title);
            Assert.AreEqual("Task details", task.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), task.DueDate);
            Assert.AreEqual(false, task.CompletedDateTime.HasValue);
            Assert.AreEqual(0, task.PriorityLevel);
        }

        [TestMethod]
        public void WhenMappingACompletedTaskModelToATask_ExpectPropertiesToMatch()
        {
            // Arrage
            var taskModel = new TaskModel
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                IsComplete = true
            };

            // Act
            var task = Mapper.Map<Task>(taskModel);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.Id);
            Assert.AreEqual("A task", task.Title);
            Assert.AreEqual("Task details", task.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), task.DueDate);
            Assert.AreEqual(false, task.CompletedDateTime.HasValue);
        }

        [TestMethod]
        public void WhenMappingAHighPriorityTaskToTaskModel_ExpectPropertiesToMatch()
        {
            // Arrage
            var task = new Task
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                PriorityLevel = 1
            };

            // Act
            var taskModel = Mapper.Map<TaskModel>(task);

            // Assert
            Assert.IsNotNull(taskModel);
            Assert.AreEqual(1, taskModel.Id);
            Assert.AreEqual("A task", taskModel.Title);
            Assert.AreEqual("Task details", taskModel.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), taskModel.DueDate);
            Assert.AreEqual(false, taskModel.IsComplete);
            Assert.AreEqual(TaskPriority.High, taskModel.PriorityLevel);
        }

        [TestMethod]
        public void WhenMappingALowPriorityTaskToTaskModel_ExpectPropertiesToMatch()
        {
            // Arrage
            var task = new Task
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                PriorityLevel = 2
            };

            // Act
            var taskModel = Mapper.Map<TaskModel>(task);

            // Assert
            Assert.IsNotNull(taskModel);
            Assert.AreEqual(1, taskModel.Id);
            Assert.AreEqual("A task", taskModel.Title);
            Assert.AreEqual("Task details", taskModel.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), taskModel.DueDate);
            Assert.AreEqual(false, taskModel.IsComplete);
            Assert.AreEqual(TaskPriority.Low, taskModel.PriorityLevel);
        }

        [TestMethod]
        public void WhenMappingAHighTaskModelToATask_ExpectPropertiesToMatch()
        {
            // Arrage
            var taskModel = new TaskModel
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                PriorityLevel = TaskPriority.High
            };

            // Act
            var task = Mapper.Map<Task>(taskModel);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.Id);
            Assert.AreEqual("A task", task.Title);
            Assert.AreEqual("Task details", task.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), task.DueDate);
            Assert.AreEqual(false, task.CompletedDateTime.HasValue);
            Assert.AreEqual(1, task.PriorityLevel);
        }

        [TestMethod]
        public void WhenMappingALowTaskModelToATask_ExpectPropertiesToMatch()
        {
            // Arrage
            var taskModel = new TaskModel
            {
                Id = 1,
                Title = "A task",
                Details = "Task details",
                DueDate = new DateTime(2014, 11, 1),
                PriorityLevel = TaskPriority.Low
            };

            // Act
            var task = Mapper.Map<Task>(taskModel);

            // Assert
            Assert.IsNotNull(task);
            Assert.AreEqual(1, task.Id);
            Assert.AreEqual("A task", task.Title);
            Assert.AreEqual("Task details", task.Details);
            Assert.AreEqual(new DateTime(2014, 11, 1), task.DueDate);
            Assert.AreEqual(false, task.CompletedDateTime.HasValue);
            Assert.AreEqual(2, task.PriorityLevel);
        }
    }
}
