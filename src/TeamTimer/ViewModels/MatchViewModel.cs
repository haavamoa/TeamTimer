using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TeamTimer.ViewModels
{
    public class MatchViewModel : BaseViewModel, IMatchViewModel, IDisposable
    {
        public MatchViewModel()
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>();
            NonPlayingPlayers = new ObservableCollection<PlayerViewModel>();
            StartMatchCommand = new Command(StartMatch);
            PauseMatchCommand = new Command(PauseMatch);
            MarkPlayerForSubCommand = new Command(MarkPlayerForSub);
            m_timer = new Timer() { Interval = 1000 };
            m_matchDuration = string.Empty;
            m_timer.Elapsed += OnEachMatchSecond;
        }

        private void PauseMatch()
        {
            m_timer.Enabled = false;
            IsMatchStarted = false;
        }

        private void StartMatch()
        {
            m_timer.Enabled = true;
            IsMatchStarted = true;
        }

        private void OnEachMatchSecond(object sender, ElapsedEventArgs e)
        {
            m_matchDurationSeconds += 1;
            MatchDuration = TimeSpan.FromSeconds(m_matchDurationSeconds).ToString();

            PlayingPlayers.ForEach(p => p.PlayTimeInSeconds += 1);
            SortPlayingPlayers();
        }

        private string m_matchDuration;

        public string MatchDuration
        {
            get => m_matchDuration;
            set => SetProperty(ref m_matchDuration, value);
        }

        private void SortPlayingPlayers()
        {
            PlayingPlayers = new ObservableCollection<PlayerViewModel>(PlayingPlayers.OrderByDescending(p => p.PlayTimeInSeconds));
            OnPropertyChanged(nameof(PlayingPlayers));
        }

        private bool ShouldSubstitute => PlayingPlayers.Any(p => p.IsMarkedForSubstitution) && NonPlayingPlayers.Any(p => p.IsMarkedForSubstitution);

        public ObservableCollection<PlayerViewModel> PlayingPlayers { get; private set; }
        public ObservableCollection<PlayerViewModel> NonPlayingPlayers { get; private set; }
        public ICommand MarkPlayerForSubCommand { get; }
        private bool m_isMatchStarted;
        private readonly Timer m_timer;
        private int m_matchDurationSeconds;

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

        private void MarkPlayerForSub(object obj)
        {
            if (!(obj is PlayerViewModel player)) return;

            if (PlayingPlayers.Contains(player))
                DeMarkEveryoneExcept(PlayingPlayers, player);
            else
                DeMarkEveryoneExcept(NonPlayingPlayers, player);

            player.IsMarkedForSubstitution = !player.IsMarkedForSubstitution;

            if (ShouldSubstitute) Substitute();
        }

        private static void DeMarkEveryoneExcept(IEnumerable<PlayerViewModel> listOfPlayers, PlayerViewModel thePlayerToIgnore)
        {
            listOfPlayers.Where(p => !p.Equals(thePlayerToIgnore)).ForEach(p => p.IsMarkedForSubstitution = false);
        }

        private void Substitute()
        {
            PlayerViewModel? playingPlayerToSub = null;
            foreach (var playingPlayer in PlayingPlayers)
                if (playingPlayer.IsMarkedForSubstitution)
                    playingPlayerToSub = playingPlayer;

            PlayerViewModel? nonPlayingPlayerToSub = null;
            foreach (var nonPlayingPlayer in NonPlayingPlayers)
                if (nonPlayingPlayer.IsMarkedForSubstitution)
                    nonPlayingPlayerToSub = nonPlayingPlayer;

            if (playingPlayerToSub != null && nonPlayingPlayerToSub != null)
            {
                PlayingPlayers.Remove(playingPlayerToSub);
                NonPlayingPlayers.Remove(nonPlayingPlayerToSub);

                PlayingPlayers.Add(nonPlayingPlayerToSub);
                NonPlayingPlayers.Add(playingPlayerToSub);

                playingPlayerToSub.IsMarkedForSubstitution = false;
                nonPlayingPlayerToSub.IsMarkedForSubstitution = false;

                SortPlayingPlayers();

                OnPropertyChanged(nameof(PlayingPlayers));
                OnPropertyChanged(nameof(NonPlayingPlayers));
            }
        }

        public void Dispose()
        {
            m_timer.Elapsed -= OnEachMatchSecond;
        }
    }
}