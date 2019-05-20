using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IMatchViewModel
    {
        ObservableCollection<PlayerViewModel> PlayingPlayers { get; }
        ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; }
        ICommand MarkPlayerForSubCommand { get; }
        Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers);
    }
}