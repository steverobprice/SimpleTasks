using SimpleTasks.Services;
using SimpleTasks.Services.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SimpleTasks.Web.Controllers
{
    public class TaskController : ApiController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //
        // GET: /api/task
        public IEnumerable<TaskModel> GetAll()
        {
            IEnumerable<TaskModel> tasks;

            if (User.Identity.IsAuthenticated)
            {
                tasks = _taskService.GetAll();
            }
            else
            {
                tasks = _taskService.GetAllOutstanding();
            }
            
            return tasks;
        }

        //
        // GET: /api/task/id
        [Authorize]
        public TaskModel Get(int id)
        {
            return _taskService.GetById(id);
        }

        //
        // POST: /api/task
        [Authorize]
        public IHttpActionResult Post(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task = _taskService.CreateTask(task);

                if (task != null)
                {
                    return Ok();
                }
            }

            return BadRequest("Unable to create new task");
        }

        //
        // PUT: /api/task
        [Authorize]
        public IHttpActionResult Put(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task = _taskService.EditTask(task);

                if (task != null)
                {
                    return Ok();
                }
            }

            return BadRequest(string.Format("Unable to update task {0}", task.Id));
        }

        //
        // DELETE: /api/task/id
        [Authorize]
        public IHttpActionResult Delete(int id)
        {
            _taskService.DeleteTask(id);
            
            return Ok();
        }

        //
        // POST: /api/task/id/complete
        [Authorize]
        [HttpPost]
        public IHttpActionResult Complete(int id)
        {
            var task = _taskService.CompleteTask(id);

            if (task != null)
            {
                return Ok();
            }

            return BadRequest(string.Format("Unable to complete task {0}", id));
        }
    }
}
