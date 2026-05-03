using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class TodoPage : ContentPage
    {
        private readonly TodoViewModel _vm;

        public TodoPage(TodoViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadTasksCommand.Execute(null);
        }
    }
}