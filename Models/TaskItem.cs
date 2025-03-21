using Microsoft.EntityFrameworkCore;

namespace TaskItClient.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? Labels { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Low;

        public DbSet<TaskItem>? Tasks { get; set; }

    }

    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public enum PriorityLevel
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
