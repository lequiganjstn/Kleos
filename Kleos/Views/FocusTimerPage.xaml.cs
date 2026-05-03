using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class FocusTimerPage : ContentPage
    {
        public FocusTimerPage(FocusTimerViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}