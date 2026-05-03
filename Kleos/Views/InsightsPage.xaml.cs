using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class InsightsPage : ContentPage
    {
        private readonly InsightsViewModel _vm;

        public InsightsPage(InsightsViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadInsightsCommand.Execute(null);
        }
    }
}