using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Services;
using Kleos.Views;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool showPassword = false;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Login";
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;
            ErrorMessage = string.Empty;
            IsBusy = true;

            try
            {
                var (success, message, user) = await _authService.LoginAsync(Email, Password);
                if (success && user is not null)
                {
                    _authService.SetCurrentUser(user);
                    await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
                }
                else
                {
                    ErrorMessage = message;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        [RelayCommand]
        private void ToggleShowPassword()
        {
            ShowPassword = !ShowPassword;
        }
    }
}