using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class CompletedPage : ContentPage
    {
        private readonly CompletedViewModel _vm;

        public CompletedPage(CompletedViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadCompletedCommand.Execute(null);
        }
    }
}