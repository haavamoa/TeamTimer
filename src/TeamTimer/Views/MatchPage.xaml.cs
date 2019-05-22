using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLToolkit.Forms.Controls;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamTimer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MatchPage : ContentPage
    {
        public MatchPage(IMatchViewModel matchViewModel)
        {
            InitializeComponent();

            BindingContext = matchViewModel;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var deviceWidth = DeviceDisplay.MainDisplayInfo.Width;
            ((FlowListView)playingPlayers).FlowColumnCount = (int)deviceWidth / 250;
            ((FlowListView)nonPlayingPlayers).FlowColumnCount = (int)deviceWidth / 250;
        }
    }
}