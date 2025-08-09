using DJZeus_Client.ViewModels;

namespace DJZeus_Client.Views
{
    public partial class MainView : ContentPage
    {
        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        
    }

}
