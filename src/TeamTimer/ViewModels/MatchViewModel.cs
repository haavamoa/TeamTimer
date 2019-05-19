using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TeamTimer.ViewModels.Interfaces.ViewModels;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : IMatchViewModel
    {
        public MatchViewModel()
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>();    
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
        }

        public ObservableCollection<PlayerViewModel> PlayingPlayers { get; private set; }
        public ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; private set; }

        public Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers)
        {
            PlayingPlayers =new ObservableCollection<PlayerViewModel>(playingPlayers);
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(nonPlayingPlayers);
            return Task.CompletedTask;
        }
    }
}