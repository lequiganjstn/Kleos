using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Data;
using Kleos.Models;
using Kleos.Services;
using System;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class FocusTimerViewModel : BaseViewModel
    {
        private readonly DatabaseService _db;
        private readonly IAuthService _authService;
        private IDispatcherTimer? _timer;

        [ObservableProperty] private int selectedMinutes = 25;
        [ObservableProperty] private int remainingSeconds;
        [ObservableProperty] private bool isRunning;
        [ObservableProperty] private bool isPaused;
        [ObservableProperty] private bool isFinished;
        [ObservableProperty] private double progress = 1.0;
        [ObservableProperty] private string displayTime = "25:00";
        [ObservableProperty] private int sessionsToday;

        private int _totalSeconds;

        public int[] TimerOptions { get; } = { 15, 20, 25, 30, 45, 60 };

        public FocusTimerViewModel(DatabaseService db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
            Title = "Focus Timer";
            ResetTimer();
        }

        partial void OnSelectedMinutesChanged(int value) => ResetTimer();

        private void ResetTimer()
        {
            _totalSeconds = SelectedMinutes * 60;
            RemainingSeconds = _totalSeconds;
            UpdateDisplay();
            Progress = 1.0;
            IsFinished = false;
        }

        [RelayCommand]
        private void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            IsPaused = false;
            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTick;
            _timer.Start();
        }

        [RelayCommand]
        private void Pause()
        {
            _timer?.Stop();
            IsRunning = false;
            IsPaused = true;
        }

        [RelayCommand]
        private void Resume()
        {
            if (!IsPaused) return;
            IsRunning = true;
            IsPaused = false;
            _timer?.Start();
        }

        [RelayCommand]
        private async Task StopAsync()
        {
            _timer?.Stop();
            var elapsed = _totalSeconds - RemainingSeconds;
            if (elapsed > 60)
                await SaveSessionAsync(elapsed / 60, false);
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
            ResetTimer();
        }

        [RelayCommand]
        private void ResetCommand()
        {
            _timer?.Stop();
            IsRunning = false;
            IsPaused = false;
            IsFinished = false;
            ResetTimer();
        }

        private async void OnTick(object? sender, EventArgs e)
        {
            RemainingSeconds--;
            Progress = (double)RemainingSeconds / _totalSeconds;
            UpdateDisplay();

            if (RemainingSeconds <= 0)
            {
                _timer?.Stop();
                IsRunning = false;
                IsFinished = true;
                await SaveSessionAsync(SelectedMinutes, true);
            }
        }

        private void UpdateDisplay()
        {
            var minutes = RemainingSeconds / 60;
            var seconds = RemainingSeconds % 60;
            DisplayTime = $"{minutes:D2}:{seconds:D2}";
        }

        private async Task SaveSessionAsync(int minutes, bool completed)
        {
            var user = _authService.GetCurrentUser();
            if (user is null) return;

            var session = new FocusSession
            {
                UserId = user.Id,
                Date = DateTime.Now,
                DurationMinutes = minutes,
                WasCompleted = completed
            };
            await _db.SaveFocusSessionAsync(session);
            SessionsToday++;
        }
    }
}