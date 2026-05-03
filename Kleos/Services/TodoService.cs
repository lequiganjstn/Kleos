using Kleos.Data;
using Kleos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public class TodoService : ITodoService
    {
        private readonly DatabaseService _db;
        private readonly IStreakService _streakService;

        public TodoService(DatabaseService db, IStreakService streakService)
        {
            _db = db;
            _streakService = streakService;
        }

        public Task<List<TodoItem>> GetActiveTasksAsync(int userId)
            => _db.GetTodoItemsAsync(userId);

        public Task<List<TodoItem>> GetCompletedTasksAsync(int userId)
            => _db.GetCompletedItemsAsync(userId);

        public Task<List<TodoItem>> GetAllTasksAsync(int userId)
            => _db.GetAllItemsAsync(userId);

        public Task SaveTaskAsync(TodoItem item)
            => _db.SaveTodoItemAsync(item);

        public Task DeleteTaskAsync(TodoItem item)
            => _db.DeleteTodoItemAsync(item);

        public async Task CompleteTaskAsync(TodoItem item)
        {
            item.IsCompleted = true;
            item.CompletedAt = DateTime.Now;
            await _db.SaveTodoItemAsync(item);
            await _streakService.RecordActivityAsync(item.UserId);
        }
    }
}