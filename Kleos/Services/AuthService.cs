using Kleos.Data;
using Kleos.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseService _db;
        private User? _currentUser;

        public AuthService(DatabaseService db)
        {
            _db = db;
        }

        public bool IsLoggedIn => _currentUser is not null;

        public async Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Full name is required.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return (false, "A valid email is required.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return (false, "Password must be at least 6 characters.");

            var existing = await _db.GetUserByEmailAsync(email.ToLower().Trim());
            if (existing is not null)
                return (false, "An account with this email already exists.");

            var user = new User
            {
                FullName = fullName.Trim(),
                Email = email.ToLower().Trim(),
                PasswordHash = HashPassword(password),
                CreatedAt = DateTime.Now
            };

            await _db.SaveUserAsync(user);
            return (true, "Account created successfully.");
        }

        public async Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "Please enter your email and password.", null);

            var user = await _db.GetUserByEmailAsync(email.ToLower().Trim());
            if (user is null)
                return (false, "No account found with this email.", null);

            if (user.PasswordHash != HashPassword(password))
                return (false, "Incorrect password.", null);

            _currentUser = user;
            Preferences.Set("logged_in_user_id", user.Id);
            return (true, "Welcome back!", user);
        }

        public void SetCurrentUser(User user) => _currentUser = user;

        public User? GetCurrentUser() => _currentUser;

        public void Logout()
        {
            _currentUser = null;
            Preferences.Remove("logged_in_user_id");
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}