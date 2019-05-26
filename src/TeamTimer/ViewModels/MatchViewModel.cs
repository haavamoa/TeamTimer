using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TeamTimer.Helpers;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : BaseViewModel, IMatchViewModel, IDisposable
    {
        public Timer Timer { get; }
        private bool m_isMatchStarted;
        private int m_matchDurationSeconds;
        private ObservableCollection<PlayerViewModel> m_playingPlayers;
        private ObservableCollection<PlayerViewModel> m_nonPlayingPlayers;

        public MatchViewModel()
        {
            m_playingPlayers = new ObservableCollection<PlayerViewModel>();
            m_nonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
            StartMatchCommand = new Command(StartMatch);
            PauseMatchCommand = new Command(PauseMatch);
            Timer = new Timer() { Interval = 1000 };
            Timer.Elapsed += OnEachMatchSecond;
        }

        private bool ShouldSubstitute => PlayingPlayers.Any(p => p.IsMarkedForSubstitution) && NonPlayingPlayers.Any(p => p.IsMarkedForSubstitution);

        public string MatchDuration => TimeSpan.FromSeconds(m_matchDurationSeconds).ToShortForm();

        public ObservableCollection<PlayerViewModel> PlayingPlayers
        {
            get => m_playingPlayers;
            set => SetProperty(ref m_playingPlayers, value);
        }

        public ObservableCollection<PlayerViewModel> NonPlayingPlayers
        {
            get => m_nonPlayingPlayers;
            set => SetProperty(ref m_nonPlayingPlayers, value);
        }

        public bool IsMatchStarted
        {
            get => m_isMatchStarted;
            set => SetProperty(ref m_isMatchStarted, value);
        }

        public ICommand StartMatchCommand { get; }
        public ICommand PauseMatchCommand { get; }

        public Task Initialize(List<PlayerViewModel> playingPlayers, List<PlayerViewModel> nonPlayingPlayers)
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>(playingPlayers);
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(nonPlayingPlayers);
            return Task.CompletedTask;
        }

        public void UpdateMatchDuration(int seconds)
        {
            m_matchDurationSeconds += seconds;
            PlayingPlayers.ForEach(p => p.PlayTimeInSeconds += seconds);
            OnPropertyChanged(nameof(PlayingPlayers));
            OnPropertyChanged(nameof(MatchDuration));
        }

        public void Dispose()
        {
            Timer.Elapsed -= OnEachMatchSecond;
        }


        public void OnPlayerChanged(PlayerViewModel changedPlayer)
        {
            if (PlayingPlayers.Contains(changedPlayer))
            {
                if (!changedPlayer.IsPlaying)
                {
                    PlayingPlayers.Remove(changedPlayer);
                    NonPlayingPlayers.Add(changedPlayer);
                }
            }
            else
            {
                if (!changedPlayer.IsPlaying) return;
                PlayingPlayers.Add(changedPlayer);
                NonPlayingPlayers.Remove(changedPlayer);
            }

            var tempPlayingPlayers = PlayingPlayers.ToList();
            var tempNonPlayingPlayers = NonPlayingPlayers.ToList();

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

        private void PauseMatch()
        {
            Timer.Enabled = false;
            IsMatchStarted = false;
        }

        private void StartMatch()
        {
            Timer.Enabled = true;
            IsMatchStarted = true;
        }

        private void OnEachMatchSecond(object sender, ElapsedEventArgs e)
        {
            m_matchDurationSeconds += 1;
            OnPropertyChanged(nameof(MatchDuration));

            PlayingPlayers.ForEach(p => p.PlayTimeInSeconds += 1);
        }

        private void MarkPlayerForSub(PlayerViewModel markedPlayer)
        {

            if (PlayingPlayers.Contains(markedPlayer))
                DeMarkEveryoneExcept(PlayingPlayers, markedPlayer);
            else
                DeMarkEveryoneExcept(NonPlayingPlayers, markedPlayer);

            markedPlayer.IsMarkedForSubstitution = !markedPlayer.IsMarkedForSubstitution;

            if (ShouldSubstitute) Substitute();
        }

        private static void DeMarkEveryoneExcept(IEnumerable<PlayerViewModel> listOfPlayers, PlayerViewModel thePlayerToIgnore)
        {
            listOfPlayers.Where(p => !p.Equals(thePlayerToIgnore)).ForEach(p => p.IsMarkedForSubstitution = false);
        }

        private void Substitute()
        {
            var tempPlayingPlayers = PlayingPlayers.ToList();
            var tempNonPlayingPlayers = NonPlayingPlayers.ToList();
            PlayerViewModel? playingPlayerToSub = null;
            foreach (var playingPlayer in tempPlayingPlayers)
                if (playingPlayer.IsMarkedForSubstitution)
                    playingPlayerToSub = playingPlayer;

            PlayerViewModel? nonPlayingPlayerToSub = null;
            foreach (var nonPlayingPlayer in tempNonPlayingPlayers)
                if (nonPlayingPlayer.IsMarkedForSubstitution)
                    nonPlayingPlayerToSub = nonPlayingPlayer;

            if (playingPlayerToSub != null && nonPlayingPlayerToSub != null)
            {
                tempPlayingPlayers.Remove(playingPlayerToSub);
                tempNonPlayingPlayers.Remove(nonPlayingPlayerToSub);

                tempPlayingPlayers.Add(nonPlayingPlayerToSub);
                tempNonPlayingPlayers.Add(playingPlayerToSub);

                playingPlayerToSub.IsMarkedForSubstitution = false;
                nonPlayingPlayerToSub.IsMarkedForSubstitution = false;

                tempPlayingPlayers = tempPlayingPlayers.OrderByDescending(p => p.PlayTimeInSeconds).ToList();
                tempPlayingPlayers.MoveLockedToEnd();
                tempNonPlayingPlayers.MoveLockedToEnd();

                PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
                NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(tempNonPlayingPlayers);
            }
        }
    }
}