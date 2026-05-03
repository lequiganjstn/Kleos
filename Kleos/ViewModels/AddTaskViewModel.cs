using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Models;
using Kleos.Services;
using Microsoft.VisualBasic;
using System;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class AddTaskViewModel : BaseViewModel
    {
        private readonly ITodoService _todoService;
        private readonly IAuthService _authService;

        [ObservableProperty] private string taskTitle = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private int priority = 1;
        [ObservableProperty] private string category = string.Empty;
        [ObservableProperty] private DateTime? dueDate;
        [ObservableProperty] private bool hasReminder;
        [ObservableProperty] private string errorMessage = string.Empty;

        public string[] PriorityOptions { get; } = { "Low", "Medium", "High" };

        public AddTaskViewModel(ITodoService todoService, IAuthService authService)
        {
            _todoService = todoService;
            _authService = authService;
            Title = "New Task";
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (IsBusy) return;
            if (string.IsNullOrWhiteSpace(TaskTitle))
            {
                ErrorMessage = "Task title cannot be empty.";
                return;
            }

            IsBusy = true;
            try
            {
                var user = _authService.GetCurrentUser();
                if (user is null) return;

                var item = new TodoItem
                {
                    Title = TaskTitle.Trim(),
                    Description = Description.Trim(),
                    Priority = Priority,
                    Category = Category.Trim(),
                    DueDate = DueDate,
                    HasReminder = HasReminder,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now
                };

                await _todoService.SaveTaskAsync(item);
                await Shell.Current.GoToAsync("..");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void SetPriority(string value)
        {
            if (int.TryParse(value, out int p))
                Priority = p;
        }
    }
}