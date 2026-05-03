using CommunityToolkit.Maui;
using Kleos.Data;
using Kleos.Services;
using Kleos.ViewModels;
using Kleos.Views;
using Microsoft.Extensions.Logging;

namespace Kleos
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                    fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                });

            // Services
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IStreakService, StreakService>();
            builder.Services.AddSingleton<ITodoService, TodoService>();

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<TodoViewModel>();
            builder.Services.AddTransient<CompletedViewModel>();
            builder.Services.AddTransient<AddTaskViewModel>();
            builder.Services.AddTransient<EditTaskViewModel>();
            builder.Services.AddTransient<StreaksViewModel>();
            builder.Services.AddTransient<FocusTimerViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<InsightsViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<TodoPage>();
            builder.Services.AddTransient<CompletedPage>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<EditTaskPage>();
            builder.Services.AddTransient<StreaksPage>();
            builder.Services.AddTransient<FocusTimerPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<InsightsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}