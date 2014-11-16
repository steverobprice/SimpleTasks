using SimpleTasks.Data.Models;

namespace SimpleTasks.Services.Models
{
    public static class TaskExtensions
    {
        public static bool IsComplete(this Task task)
        {
            return task.CompletedDateTime.HasValue;
        }
    }
}
