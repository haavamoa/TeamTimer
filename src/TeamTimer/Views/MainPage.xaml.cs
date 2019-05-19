using System;
using TeamTimer.ViewModels;
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

            BindingContext = mainViewModel;
            
        }

        private void AddPlayerButton_Clicked(object sender, EventArgs e)
        {
            NewPlayerEntry.Focus();
        }

        private void PlayerViewModel_Tapped(object sender, EventArgs e)
        {
            if (sender is ViewCell tappedViewCell)
            {
                if (tappedViewCell.BindingContext is PlayerViewModel tappedPlayerViewModel)
                {
                    tappedPlayerViewModel.IsPlaying = !tappedPlayerViewModel.IsPlaying;
                }
            }
            
        }
    }
}
