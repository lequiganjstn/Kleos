using Kleos.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kleos.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetActiveTasksAsync(int userId);
        Task<List<TodoItem>> GetCompletedTasksAsync(int userId);
        Task<List<TodoItem>> GetAllTasksAsync(int userId);
        Task SaveTaskAsync(TodoItem item);
        Task DeleteTaskAsync(TodoItem item);
        Task CompleteTaskAsync(TodoItem item);
    }
}