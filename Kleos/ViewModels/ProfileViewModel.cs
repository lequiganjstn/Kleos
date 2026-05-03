using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kleos.Services;
using Kleos.Views;
using System.Threading.Tasks;

namespace Kleos.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string fullName = string.Empty;
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string avatarInitials = string.Empty;

        public ProfileViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Profile";
        }

        [RelayCommand]
        private void LoadProfile()
        {
            var user = _authService.GetCurrentUser();
            if (user is null) return;
            FullName = user.FullName;
            Email = user.Email;
            AvatarInitials = user.AvatarInitials;
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            var confirm = await Shell.Current.DisplayAlert("Logout",
                "Are you sure you want to logout?", "Logout", "Cancel");
            if (!confirm) return;
            _authService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}