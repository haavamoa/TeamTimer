using System;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;

namespace TeamTimer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IMainViewModel mainViewModel)
        {
            InitializeComponent();

            mainViewModel.Navigation = Navigation;
            BindingContext = mainViewModel;
            
        }

        private void AddPlayerButton_Clicked(object sender, EventArgs e)
        {
            NewPlayerEntry.Focus();
        }
    }
}
