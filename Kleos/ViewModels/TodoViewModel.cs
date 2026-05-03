using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Models;
using Kleos.Services;
using Kleos.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class TodoViewModel : BaseViewModel
    {
        private readonly ITodoService _todoService;
        private readonly IAuthService _authService;

        [ObservableProperty] private ObservableCollection<TodoItem> tasks = new();
        [ObservableProperty] private string greeting = string.Empty;
        [ObservableProperty] private int pendingCount;
        [ObservableProperty] private bool hasNoTasks;

        public TodoViewModel(ITodoService todoService, IAuthService authService)
        {
            _todoService = todoService;
            _authService = authService;
            Title = "My Tasks";
        }

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var user = _authService.GetCurrentUser();
                if (user is null) return;

                var hour = System.DateTime.Now.Hour;
                Greeting = hour < 12 ? $"Good morning, {user.FullName.Split(' ')[0]}."
                    : hour < 17 ? $"Good afternoon, {user.FullName.Split(' ')[0]}."
                    : $"Good evening, {user.FullName.Split(' ')[0]}.";

                var items = await _todoService.GetActiveTasksAsync(user.Id);
                Tasks.Clear();
                foreach (var item in items) Tasks.Add(item);
                PendingCount = Tasks.Count;
                HasNoTasks = Tasks.Count == 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CompleteTaskAsync(TodoItem item)
        {
            await _todoService.CompleteTaskAsync(item);
            await LoadTasksAsync();
        }

        [RelayCommand]
        private async Task DeleteTaskAsync(TodoItem item)
        {
            var confirm = await Shell.Current.DisplayAlert("Delete Task",
                $"Delete \"{item.Title}\"?", "Delete", "Cancel");
            if (!confirm) return;
            await _todoService.DeleteTaskAsync(item);
            await LoadTasksAsync();
        }

        [RelayCommand]
        private async Task GoToEditTaskAsync(TodoItem item)
        {
            await Shell.Current.GoToAsync(nameof(EditTaskPage),
                new Dictionary<string, object> { { "Task", item } });
        }

        [RelayCommand]
        private async Task GoToAddTaskAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddTaskPage));
        }

        [RelayCommand]
        private async Task GoToFocusTimerAsync()
        {
            await Shell.Current.GoToAsync(nameof(FocusTimerPage));
        }
    }
}