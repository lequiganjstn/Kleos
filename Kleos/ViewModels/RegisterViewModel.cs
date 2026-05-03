using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Services;
using Kleos.Views;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string fullName = string.Empty;
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string confirmPassword = string.Empty;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool showPassword = false;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Register";
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsBusy) return;
            ErrorMessage = string.Empty;

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            IsBusy = true;
            try
            {
                var (success, message) = await _authService.RegisterAsync(FullName, Email, Password);
                if (success)
                    await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
                else
                    ErrorMessage = message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void ToggleShowPassword() => ShowPassword = !ShowPassword;
    }
}