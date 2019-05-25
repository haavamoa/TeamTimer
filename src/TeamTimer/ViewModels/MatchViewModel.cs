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

        public MatchViewModel()
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>();
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
            StartMatchCommand = new Command(StartMatch);
            PauseMatchCommand = new Command(PauseMatch);
            Timer = new Timer() { Interval = 1000 };
            Timer.Elapsed += OnEachMatchSecond;
        }

        private bool ShouldSubstitute => PlayingPlayers.Any(p => p.IsMarkedForSubstitution) && NonPlayingPlayers.Any(p => p.IsMarkedForSubstitution);

        public string MatchDuration => TimeSpan.FromSeconds(m_matchDurationSeconds).ToShortForm();

        public ObservableCollection<PlayerViewModel> PlayingPlayers { get; private set; }
        public ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; private set; }

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

                    OnPropertyChanged(nameof(PlayingPlayers));
                    OnPropertyChanged(nameof(NonPlayingPlayers));
                }
            }
            else
            {
                if (!changedPlayer.IsPlaying) return;
                PlayingPlayers.Add(changedPlayer);
                NonPlayingPlayers.Remove(changedPlayer);

                OnPropertyChanged(nameof(PlayingPlayers));
                OnPropertyChanged(nameof(NonPlayingPlayers));
            }
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

                tempPlayingPlayers.OrderByPlayTime();
                tempPlayingPlayers.MoveLockedToEnd();
                tempNonPlayingPlayers.MoveLockedToEnd();

                PlayingPlayers = new ObservableCollection<PlayerViewModel>(tempPlayingPlayers);
                NonPlayingPlayers = new ObservableCollection<PlayerViewModel>(tempNonPlayingPlayers);
                OnPropertyChanged(nameof(PlayingPlayers));
                OnPropertyChanged(nameof(NonPlayingPlayers));
            }
        }
    }
}