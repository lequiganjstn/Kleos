using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class StreaksPage : ContentPage
    {
        private readonly StreaksViewModel _vm;

        public StreaksPage(StreaksViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadStreakCommand.Execute(null);
        }
    }
}