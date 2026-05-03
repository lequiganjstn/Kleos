using Kleos.Views;

namespace Kleos
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
            Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
            Routing.RegisterRoute(nameof(FocusTimerPage), typeof(FocusTimerPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }
    }
}