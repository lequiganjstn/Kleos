using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Models;
using Kleos.Services;
using Microsoft.VisualBasic;
using System;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    [QueryProperty(nameof(Task), "Task")]
    public partial class EditTaskViewModel : BaseViewModel
    {
        private readonly ITodoService _todoService;

        [ObservableProperty] private TodoItem? task;
        [ObservableProperty] private string taskTitle = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private int priority = 1;
        [ObservableProperty] private string category = string.Empty;
        [ObservableProperty] private DateTime? dueDate;
        [ObservableProperty] private bool hasReminder;
        [ObservableProperty] private string errorMessage = string.Empty;

        public string[] PriorityOptions { get; } = { "Low", "Medium", "High" };

        public EditTaskViewModel(ITodoService todoService)
        {
            _todoService = todoService;
            Title = "Edit Task";
        }

        partial void OnTaskChanged(TodoItem? value)
        {
            if (value is null) return;
            TaskTitle = value.Title;
            Description = value.Description;
            Priority = value.Priority;
            Category = value.Category;
            DueDate = value.DueDate;
            HasReminder = value.HasReminder;
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
            if (Task is null) return;

            IsBusy = true;
            try
            {
                Task.Title = TaskTitle.Trim();
                Task.Description = Description.Trim();
                Task.Priority = Priority;
                Task.Category = Category.Trim();
                Task.DueDate = DueDate;
                Task.HasReminder = HasReminder;

                await _todoService.SaveTaskAsync(Task);
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