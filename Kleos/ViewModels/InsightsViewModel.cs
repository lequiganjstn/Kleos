using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Data;
using Kleos.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class InsightsViewModel : BaseViewModel
    {
        private readonly ITodoService _todoService;
        private readonly DatabaseService _db;
        private readonly IAuthService _authService;

        [ObservableProperty] private int totalTasks;
        [ObservableProperty] private int completedTasks;
        [ObservableProperty] private int pendingTasks;
        [ObservableProperty] private int totalFocusMinutes;
        [ObservableProperty] private double completionRate;
        [ObservableProperty] private int completedThisWeek;
        [ObservableProperty] private int highPriorityCompleted;
        [ObservableProperty] private string topCategory = "—";

        public InsightsViewModel(ITodoService todoService, DatabaseService db, IAuthService authService)
        {
            _todoService = todoService;
            _db = db;
            _authService = authService;
            Title = "Insights";
        }

        [RelayCommand]
        private async Task LoadInsightsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var user = _authService.GetCurrentUser();
                if (user is null) return;

                var all = await _todoService.GetAllTasksAsync(user.Id);
                TotalTasks = all.Count;
                CompletedTasks = all.Count(t => t.IsCompleted);
                PendingTasks = all.Count(t => !t.IsCompleted);
                CompletionRate = TotalTasks > 0 ? Math.Round((double)CompletedTasks / TotalTasks * 100, 1) : 0;

                var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                CompletedThisWeek = all.Count(t => t.IsCompleted && t.CompletedAt >= weekStart);

                HighPriorityCompleted = all.Count(t => t.IsCompleted && t.Priority == 2);

                TotalFocusMinutes = await _db.GetTotalFocusMinutesAsync(user.Id);

                var categoryGroups = all
                    .Where(t => !string.IsNullOrWhiteSpace(t.Category) && t.IsCompleted)
                    .GroupBy(t => t.Category)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();
                TopCategory = categoryGroups?.Key ?? "—";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}