using SimpleTasks.Data.Models;
using System.Data.Entity;

namespace SimpleTasks.Data
{
    public class TasksDbContext : DbContext
    {

        public TasksDbContext()
            : base("TasksDBConnectionString")
        {
            Database.SetInitializer<TasksDbContext>(new CreateDatabaseIfNotExists<TasksDbContext>());

        }

        public DbSet<Task> Tasks { get; set; }
    }
}
