using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamTimer.Helpers;
using TeamTimer.Services;
using TeamTimer.Services.Dialog;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Navigation;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.Handlers;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : BaseViewModel, IMatchViewModel
    {
        private readonly IDialogService m_dialogService;
        private readonly INavigationService m_navigationService;
        private bool m_isMatchStarted;
        private int m_matchDurationSeconds;
        private ObservableCollection<PlayerViewModel> m_nonPlayingPlayers;
        private ObservableCollection<PlayerViewModel> m_playingPlayers;
        private IHandleTeamSetup m_teamSetupHandler;

        public MatchViewModel(IStopwatchService stopwatchService, INavigationService navigationService, IDialogService dialogService)
        {
            m_navigationService = navigationService;
            m_dialogService = dialogService;
            StopwatchService = stopwatchService;
            m_playingPlayers = new ObservableCollection<PlayerViewModel>();
            m_nonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
            StartMatchCommand = new Command(StartMatch);
            StopMatchCommand = new Command(TryStopMatch);
            PauseMatchCommand = new Command(PauseMatch);
            stopwatchService.RegisterInterval(1000, OnEachMatchSecond);
        }

        private bool ShouldSubstitute => PlayingPlayers.Any(p => p.IsMarkedForSubstitution) && NonPlayingPlayers.Any(p => p.IsMarkedForSubstitution);

        public ICommand StopMatchCommand { get; private set; }

        public IStopwatchService StopwatchService { get; }

        public string MatchDuration => TimeSpan.FromSeconds(m_matchDurationSeconds).ToShortForm();

        public ObservableCollection<PlayerViewModel> PlayingPlayers
        {
            get => m_playingPlayers;
            private set => SetProperty(ref m_playingPlayers, value);
        }

        public ObservableCollection<PlayerViewModel> NonPlayingPlayers
        {
            get => m_nonPlayingPlayers;
            private set => SetProperty(ref m_nonPlayingPlayers, value);
        }

        public bool IsMatchStarted
        {
            get => m_isMatchStarted;
            private set => SetProperty(ref m_isMatchStarted, value);
        }

        public ICommand StartMatchCommand { get; }
        public ICommand PauseMatchCommand { get; }

        public Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers, IHandleTeamSetup teamSetupHandler)
        {
            var orderedPlayingPlayers = playingPlayers.OrderByDescending(p => p.PlayTimeInSeconds).ToList();
            orderedPlayingPlayers.MoveLockedToEnd();
            PlayingPlayers = new ObservableCollection<PlayerViewModel>(orderedPlayingPlayers);

            nonPlayingPlayers.MoveLockedToEnd();
            if (NonPlayingPlayers.Any())
            {
                NonPlayingPlayers.AddPlayersToExisting(nonPlayingPlayers);
            }
            else
            {
                NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(nonPlayingPlayers);
            }

            OnPropertyChanged(nameof(NonPlayingPlayers));
            m_teamSetupHandler = teamSetupHandler;
            return Task.CompletedTask;
        }

        public void UpdateMatchDuration(int seconds)
        {
            m_matchDurationSeconds += seconds;
            PlayingPlayers.ForEach(p => p.PlayTimeInSeconds += seconds);
            OnPropertyChanged(nameof(PlayingPlayers));
            OnPropertyChanged(nameof(MatchDuration));
        }

        public void OnPlayerChanged(PlayerViewModel changedPlayer)
        {
            var tempPlayingPlayers = PlayingPlayers.ToList();
            var tempNonPlayingPlayers = NonPlayingPlayers.ToList();

            if (tempPlayingPlayers.Contains(changedPlayer))
            {
                if (!changedPlayer.IsPlaying)
                {
                    tempPlayingPlayers.Remove(changedPlayer);
                    tempNonPlayingPlayers.Add(changedPlayer);
                }
            }
            else
            {
                if (changedPlayer.IsPlaying)
                {
                    tempPlayingPlayers.Add(changedPlayer);
                    tempNonPlayingPlayers.Remove(changedPlayer);
                }
            }

            tempPlayingPlayers = tempPlayingPlayers.OrderByDescending(p => p.PlayTimeInSeconds).ToList();
            tempPlayingPlayers.MoveLockedToEnd();
            tempNonPlayingPlayers.MoveLockedToEnd();

            PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(tempNonPlayingPlayers);
        }

        public void OnPlayerMarkedForSub(PlayerViewModel markedPlayer)
        {
            MarkPlayerForSub(markedPlayer);
        }

        public string Title => "Match";
        public bool IsBusy { get; set; }

        public void Dispose()
        {
            StopwatchService?.Dispose();
        }

        private void TryStopMatch()
        {
            var confirmAction = new DialogAction(
                "Yes",
                () =>
                {
                    IsMatchStarted = false;
                    StopwatchService.Pause();
                    Reset();
                    m_navigationService.NavigateBack();
                });
            m_dialogService.ShowActionSheet("Stop match?", "No", confirmAction);
        }

        private void Reset()
        {
            PlayingPlayers.ForEach(p => p.Reset());
            NonPlayingPlayers.ForEach(p => p.Reset());
            PlayingPlayers.Clear();
            NonPlayingPlayers.Clear();
            m_matchDurationSeconds = 0;
            OnPropertyChanged(nameof(MatchDuration));
        }

        private void PauseMatch()
        {
            StopwatchService.Pause();
            IsMatchStarted = false;
        }

        private void StartMatch()
        {
            StopwatchService.Start();
            IsMatchStarted = true;
        }

        private void OnEachMatchSecond()
        {
            m_matchDurationSeconds += 1;
            OnPropertyChanged(nameof(MatchDuration));

            var tempPlayingPlayers = PlayingPlayers.ToList();
            tempPlayingPlayers.ForEach(p => p.PlayTimeInSeconds += 1);
            PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
        }

        private void MarkPlayerForSub(PlayerViewModel markedPlayer)
        {
            var tempPlayingPlayers = PlayingPlayers.ToList();
            var tempNonPlayingPlayers = NonPlayingPlayers.ToList();

            if (tempPlayingPlayers.Contains(markedPlayer))
            {
                tempPlayingPlayers.DeMarkEveryoneExcept(markedPlayer);
            }
            else
            {
                tempNonPlayingPlayers.DeMarkEveryoneExcept(markedPlayer);
            }

            markedPlayer.IsMarkedForSubstitution = !markedPlayer.IsMarkedForSubstitution;

            PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(tempNonPlayingPlayers);

            if (ShouldSubstitute)
            {
                Substitute();
            }
        }

        private void Substitute()
        {
            var tempPlayingPlayers = PlayingPlayers.ToList();
            var tempNonPlayingPlayers = NonPlayingPlayers.ToList();
            var playingPlayerToSub = tempPlayingPlayers.Single(p => p.IsMarkedForSubstitution);
            var nonPlayingPlayerToSub = tempNonPlayingPlayers.Single(p => p.IsMarkedForSubstitution);

            if (playingPlayerToSub != null && nonPlayingPlayerToSub != null)
            {
                tempPlayingPlayers.Remove(playingPlayerToSub);
                tempNonPlayingPlayers.Remove(nonPlayingPlayerToSub);

                tempPlayingPlayers.Add(nonPlayingPlayerToSub);
                tempNonPlayingPlayers.Add(playingPlayerToSub);

                playingPlayerToSub.IsMarkedForSubstitution = false;
                nonPlayingPlayerToSub.IsMarkedForSubstitution = false;

                playingPlayerToSub.IsPlaying = false;
                nonPlayingPlayerToSub.IsPlaying = true;

                tempPlayingPlayers = tempPlayingPlayers.OrderByDescending(p => p.PlayTimeInSeconds).ToList();
                tempPlayingPlayers.MoveLockedToEnd();
                tempNonPlayingPlayers.MoveLockedToEnd();

                PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
                NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(tempNonPlayingPlayers);
                m_teamSetupHandler?.OnPlayerChanged(playingPlayerToSub);
            }
        }
    }
}