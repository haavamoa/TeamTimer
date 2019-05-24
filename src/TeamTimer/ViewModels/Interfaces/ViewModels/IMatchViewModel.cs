using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IMatchViewModel : IDisposable, IHandleMatch
    {
        ObservableCollection<PlayerViewModel> PlayingPlayers { get; }
        ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; }
        bool IsMatchStarted { get; }
        string MatchDuration { get; }
        ICommand StartMatchCommand { get; }
        ICommand PauseMatchCommand { get; }
        Timer Timer { get; }
        Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers);
        void UpdateMatchDuration(int seconds);
    }
}