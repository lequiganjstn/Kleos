using SQLite;
using System;

namespace Kleos.Models
{
    [Table("Streaks")]
    public class Streak
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrentStreak { get; set; } = 0;

        public int LongestStreak { get; set; } = 0;

        public DateTime LastActivityDate { get; set; } = DateTime.MinValue;

        public int TotalTasksCompleted { get; set; } = 0;
    }
}