using SQLite;
using System;

namespace Kleos.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public string Email { get; set; } = string.Empty;

        [NotNull]
        public string PasswordHash { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string AvatarInitials => FullName.Length >= 2
            ? FullName.Substring(0, 2).ToUpper()
            : FullName.ToUpper();
    }
}