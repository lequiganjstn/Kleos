using Kleos.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kleos.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "kleos.db3");
        }

        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<TodoItem>();
            await _database.CreateTableAsync<Streak>();
            await _database.CreateTableAsync<FocusSession>();
        }

        // --- User ---

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await InitAsync();
            return await _database!.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            await InitAsync();
            return await _database!.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveUserAsync(User user)
        {
            await InitAsync();
            if (user.Id == 0)
                return await _database!.InsertAsync(user);
            return await _database!.UpdateAsync(user);
        }

        // --- Todo Items ---

        public async Task<List<TodoItem>> GetTodoItemsAsync(int userId)
        {
            await InitAsync();
            return await _database!.Table<TodoItem>()
                .Where(t => t.UserId == userId && !t.IsCompleted)
                .OrderByDescending(t => t.Priority)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetCompletedItemsAsync(int userId)
        {
            await InitAsync();
            return await _database!.Table<TodoItem>()
                .Where(t => t.UserId == userId && t.IsCompleted)
                .OrderByDescending(t => t.CompletedAt)
                .ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllItemsAsync(int userId)
        {
            await InitAsync();
            return await _database!.Table<TodoItem>()
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> SaveTodoItemAsync(TodoItem item)
        {
            await InitAsync();
            if (item.Id == 0)
                return await _database!.InsertAsync(item);
            return await _database!.UpdateAsync(item);
        }

        public async Task<int> DeleteTodoItemAsync(TodoItem item)
        {
            await InitAsync();
            return await _database!.DeleteAsync(item);
        }

        public async Task<int> GetCompletedTodayCountAsync(int userId)
        {
            await InitAsync();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return await _database!.Table<TodoItem>()
                .Where(t => t.UserId == userId && t.IsCompleted
                    && t.CompletedAt >= today && t.CompletedAt < tomorrow)
                .CountAsync();
        }

        // --- Streaks ---

        public async Task<Streak?> GetStreakAsync(int userId)
        {
            await InitAsync();
            return await _database!.Table<Streak>().Where(s => s.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<int> SaveStreakAsync(Streak streak)
        {
            await InitAsync();
            if (streak.Id == 0)
                return await _database!.InsertAsync(streak);
            return await _database!.UpdateAsync(streak);
        }

        // --- Focus Sessions ---

        public async Task<int> SaveFocusSessionAsync(FocusSession session)
        {
            await InitAsync();
            return await _database!.InsertAsync(session);
        }

        public async Task<List<FocusSession>> GetFocusSessionsAsync(int userId)
        {
            await InitAsync();
            return await _database!.Table<FocusSession>()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.Date)
                .ToListAsync();
        }

        public async Task<int> GetTotalFocusMinutesAsync(int userId)
        {
            await InitAsync();
            var sessions = await _database!.Table<FocusSession>()
                .Where(f => f.UserId == userId && f.WasCompleted)
                .ToListAsync();
            int total = 0;
            foreach (var s in sessions) total += s.DurationMinutes;
            return total;
        }
    }
}