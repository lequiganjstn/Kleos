using Kleos.ViewModels;

namespace Kleos.Views
{
    public partial class EditTaskPage : ContentPage
    {
        public EditTaskPage(EditTaskViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}