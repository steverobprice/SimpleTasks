using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleTasks.Services.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? CompletedDateTime { get; set; }
        public bool IsComplete { get; set; }

        [Required]
        public string Owner { get; set; }
    }
}
