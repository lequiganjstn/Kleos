using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Services;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class StreaksViewModel : BaseViewModel
    {
        private readonly IStreakService _streakService;
        private readonly IAuthService _authService;

        [ObservableProperty] private int currentStreak;
        [ObservableProperty] private int longestStreak;
        [ObservableProperty] private int totalCompleted;
        [ObservableProperty] private bool hasBronzeBadge;
        [ObservableProperty] private bool hasSilverBadge;
        [ObservableProperty] private bool hasGoldBadge;
        [ObservableProperty] private string motivationalMessage = string.Empty;

        public StreaksViewModel(IStreakService streakService, IAuthService authService)
        {
            _streakService = streakService;
            _authService = authService;
            Title = "Streaks";
        }

        [RelayCommand]
        private async Task LoadStreakAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var user = _authService.GetCurrentUser();
                if (user is null) return;

                var streak = await _streakService.GetStreakAsync(user.Id);
                CurrentStreak = streak.CurrentStreak;
                LongestStreak = streak.LongestStreak;
                TotalCompleted = streak.TotalTasksCompleted;

                HasBronzeBadge = TotalCompleted >= 10;
                HasSilverBadge = TotalCompleted >= 50;
                HasGoldBadge = TotalCompleted >= 100;

                MotivationalMessage = CurrentStreak switch
                {
                    0 => "Complete a task to start your streak.",
                    1 => "Great start. Keep it going!",
                    >= 7 => $"Incredible! {CurrentStreak} days in a row.",
                    >= 3 => $"{CurrentStreak} days strong. You are on fire!",
                    _ => $"{CurrentStreak} day streak. Stay consistent."
                };
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}