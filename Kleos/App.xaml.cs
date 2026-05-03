using Kleos.Data;
using Kleos.Services;
using Kleos.Views;

namespace Kleos
{
    public partial class App : Application
    {
        private readonly IAuthService _authService;
        private readonly DatabaseService _db;

        public App(IAuthService authService, DatabaseService db)
        {
            InitializeComponent();
            _authService = authService;
            _db = db;

            MainPage = new AppShell();
            _ = TryAutoLoginAsync();
        }

        private async Task TryAutoLoginAsync()
        {
            var userId = Preferences.Get("logged_in_user_id", 0);
            if (userId > 0)
            {
                var user = await _db.GetUserByIdAsync(userId);
                if (user is not null)
                {
                    _authService.SetCurrentUser(user);
                    await Shell.Current.GoToAsync($"//{nameof(TodoPage)}");
                    return;
                }
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}