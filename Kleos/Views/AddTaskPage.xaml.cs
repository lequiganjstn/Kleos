using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage(AddTaskViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}