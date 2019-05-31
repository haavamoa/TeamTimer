using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TeamTimer.Services;
using TeamTimer.ViewModels.Interfaces.Handlers;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IMatchViewModel : IViewModel, IHandleMatch
    {
        ObservableCollection<PlayerViewModel> PlayingPlayers { get; }
        ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; }
        bool IsMatchStarted { get; }
        string MatchDuration { get; }
        ICommand StartMatchCommand { get; }
        ICommand PauseMatchCommand { get; }
        IStopwatchService StopwatchService { get; }
        Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers, IHandleTeamSetup teamSetupHandler);
        void UpdateMatchDuration(int seconds);
    }
}