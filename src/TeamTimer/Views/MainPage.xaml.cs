using System;
using System.Linq;
using TeamTimer.ViewModels;
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

        private void OnPlayerAdded(object sender, EventArgs e)
        {
            NewPlayerEntry.Focus();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var previous = e.PreviousSelection;
            var current = e.CurrentSelection;

            foreach (PlayerViewModel potentialDeSelectedPlayer in previous)
            {
                if (current.Contains(potentialDeSelectedPlayer))
                {
                    continue;
                }

                potentialDeSelectedPlayer.IsPlaying = false;
                return;
            }

            var lastSelected = current.LastOrDefault();
            if (lastSelected == null)
            {
                return;
            }

            if (lastSelected is PlayerViewModel selectedPlayer)
            {
                selectedPlayer.IsPlaying = true;
            }
        }

        protected override bool OnBackButtonPressed() => true;
    }
}