using System;

namespace SimpleTasks.Data.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public string Owner { get; set; }
    }
}
