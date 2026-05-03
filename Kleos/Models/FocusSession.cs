using SQLite;
using System;

namespace Kleos.Models
{
    [Table("FocusSessions")]
    public class FocusSession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int DurationMinutes { get; set; }

        public bool WasCompleted { get; set; }

        public int? LinkedTaskId { get; set; }
    }
}