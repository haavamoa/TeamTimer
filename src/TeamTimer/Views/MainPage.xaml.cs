using System;
using System.Collections.Generic;
using System.Linq;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IMainViewModel mainViewModel)
        {
            InitializeComponent();

            BindingContext = mainViewModel;
        }

        private void OnPlayerAdded(object sender, EventArgs e)
        {
            NewPlayerEntry.Focus();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var firstItem = e.CurrentSelection.LastOrDefault();
            if (firstItem != null)
            {
                if (firstItem is PlayerViewModel selectedPlayer)
                {
                    selectedPlayer.IsPlaying = !selectedPlayer.IsPlaying;
                }
            }
        }
    }
}