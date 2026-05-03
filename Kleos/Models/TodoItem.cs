using SQLite;
using System;

namespace Kleos.Models
{
    [Table("TodoItems")]
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int Priority { get; set; } = 1; // 0=Low, 1=Medium, 2=High

        public string Category { get; set; } = string.Empty;

        public int UserId { get; set; }

        public bool HasReminder { get; set; } = false;

        public DateTime? ReminderTime { get; set; }

        [Ignore]
        public string PriorityLabel => Priority switch
        {
            2 => "High",
            1 => "Medium",
            0 => "Low",
            _ => "Medium"
        };

        [Ignore]
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && !IsCompleted;
    }
}