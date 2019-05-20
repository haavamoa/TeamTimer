using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : BaseViewModel, IMatchViewModel
    {
        public MatchViewModel()
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>();
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
            MarkPlayerForSubCommand = new Command(MarkPlayerForSub);
        }

        private bool ShouldSubstitute => PlayingPlayers.Any(p => p.IsMarkedForSubstitution) && NonPlayingPlayers.Any(p => p.IsMarkedForSubstitution);

        public ObservableCollection<PlayerViewModel> PlayingPlayers { get; private set; }
        public ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; private set; }
        public ICommand MarkPlayerForSubCommand { get; }

        public Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers)
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>(playingPlayers);
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(nonPlayingPlayers);
            return Task.CompletedTask;
        }

        private void MarkPlayerForSub(object obj)
        {
            if (!(obj is PlayerViewModel player)) return;

            if (PlayingPlayers.Contains(player))
            {
                DeMarkEveryoneExcept(PlayingPlayers, player);
            }
            else
            {
                DeMarkEveryoneExcept(NonPlayingPlayers, player);
            }

            player.IsMarkedForSubstitution = !player.IsMarkedForSubstitution;

            if (ShouldSubstitute)
            {
                Substitute();
            }
        }

        private static void DeMarkEveryoneExcept(IEnumerable<PlayerViewModel> listOfPlayers, PlayerViewModel thePlayerToIgnore)
        {
            listOfPlayers.Where(p => !p.Equals(thePlayerToIgnore)).ForEach(p => p.IsMarkedForSubstitution = false);
        }

        private void Substitute()
        {
            PlayerViewModel? playingPlayerToSub = null;
            foreach (var playingPlayer in PlayingPlayers)
            {
                if (playingPlayer.IsMarkedForSubstitution)
                {
                    playingPlayerToSub = playingPlayer;
                }
            }

            PlayerViewModel? nonPlayingPlayerToSub = null;
            foreach (var nonPlayingPlayer in NonPlayingPlayers)
            {
                if (nonPlayingPlayer.IsMarkedForSubstitution)
                {
                    nonPlayingPlayerToSub = nonPlayingPlayer;
                }
            }


            if (playingPlayerToSub != null && nonPlayingPlayerToSub != null)
            {
                PlayingPlayers.Remove(playingPlayerToSub);
                NonPlayingPlayers.Remove(nonPlayingPlayerToSub);

                PlayingPlayers.Add(nonPlayingPlayerToSub);
                NonPlayingPlayers.Add(playingPlayerToSub);

                playingPlayerToSub.IsMarkedForSubstitution = false;
                nonPlayingPlayerToSub.IsMarkedForSubstitution = false;

                OnPropertyChanged(nameof(PlayingPlayers));
                OnPropertyChanged(nameof(NonPlayingPlayers));
            }
        }
    }
}