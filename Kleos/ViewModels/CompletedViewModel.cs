using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Models;
using Kleos.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class CompletedViewModel : BaseViewModel
    {
        private readonly ITodoService _todoService;
        private readonly IAuthService _authService;

        [ObservableProperty] private ObservableCollection<TodoItem> completedTasks = new();
        [ObservableProperty] private bool hasNoTasks;
        [ObservableProperty] private int completedCount;

        public CompletedViewModel(ITodoService todoService, IAuthService authService)
        {
            _todoService = todoService;
            _authService = authService;
            Title = "Completed";
        }

        [RelayCommand]
        private async Task LoadCompletedAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var user = _authService.GetCurrentUser();
                if (user is null) return;
                var items = await _todoService.GetCompletedTasksAsync(user.Id);
                CompletedTasks.Clear();
                foreach (var item in items) CompletedTasks.Add(item);
                CompletedCount = CompletedTasks.Count;
                HasNoTasks = CompletedTasks.Count == 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task DeleteTaskAsync(TodoItem item)
        {
            await _todoService.DeleteTaskAsync(item);
            await LoadCompletedAsync();
        }
    }
}