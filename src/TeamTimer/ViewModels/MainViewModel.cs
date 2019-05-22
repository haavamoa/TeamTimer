using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamTimer.Models;
using TeamTimer.Resources.Commands;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.Handlers;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using TeamTimer.Views;
using Xamarin.Forms;

namespace TeamTimer.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel, IHandleTeam
    {
        private string m_newPlayerName = string.Empty;
        private INavigation? m_navigation;

        public MainViewModel(IMatchViewModel matchViewModel)
        {
            MatchViewModel = matchViewModel;
            SaveTeamCommand = new AsyncCommand(_ => SavePlayersAndNavigate(), _ => Players.Any(p => p.IsPlaying));
            AddPlayerCommand = new Command(AddPlayerOrPlayers, () => !string.IsNullOrEmpty(NewPlayerName));
            Players = new ObservableCollection<PlayerViewModel>();
        }

        public void OnPlayerDeleted(PlayerViewModel deletedPlayer)
        {
            Players.Remove(deletedPlayer);
            OnPropertyChanged(nameof(NumberOfStartingPlayers));
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)SaveTeamCommand).ChangeCanExecute();
        }

        public void OnPlayerChanged(PlayerViewModel changedPlayer)
        {
            OnPropertyChanged(nameof(NumberOfStartingPlayers));
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)SaveTeamCommand).ChangeCanExecute();
        }

        public ObservableCollection<PlayerViewModel> Players { get; private set; }

        public ICommand AddPlayerCommand { get; private set; }


        public ICommand SaveTeamCommand { get; private set; }

        public string NewPlayerName
        {
            get => m_newPlayerName;
            set => SetProperty(ref m_newPlayerName, value, commandsToChangeCanExecute: (Command)AddPlayerCommand);
        }

        public int NumberOfStartingPlayers => Players.Count(p => p.IsPlaying);

        public Task Initialize(INavigation navigation)
        {
            m_navigation = navigation;
            return Task.CompletedTask;
        }

        public IMatchViewModel MatchViewModel { get; }

        private void AddPlayerOrPlayers()
        {
            if (IsMultipleNames(NewPlayerName, out var players))
            {
                players.ForEach(p => AddPlayer(p.Name));
            }
            else
            {
                AddPlayer(NewPlayerName);
            }

            NewPlayerName = string.Empty;

            Players = new ObservableCollection<PlayerViewModel>(Players.OrderBy(p => p.Name));
            OnPropertyChanged(nameof(Players));

            OnPropertyChanged(nameof(NumberOfStartingPlayers));
        }

        private void AddPlayer(string newPlayerName)
        {
            var newPlayer = new PlayerViewModel(new Player(newPlayerName));
            newPlayer.Initialize(this);
            Players.Add(newPlayer);
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)SaveTeamCommand).ChangeCanExecute();
        }

        private static bool IsMultipleNames(string newPlayerName, out List<PlayerViewModel> players)
        {
            var newPlayerNames = newPlayerName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            players = new List<PlayerViewModel>();
            players.AddRange(newPlayerNames.Select(newPlayer => new PlayerViewModel(new Player(newPlayer))));
            return players.Any();
        }

        private async Task SavePlayersAndNavigate()
        {
            if (m_navigation != null)
            {
                var currentPage = m_navigation.NavigationStack.LastOrDefault();
                switch (currentPage)
                {
                    case null:
                        await m_navigation.PushAsync(new MainPage(this));
                        break;
                    case MainPage _:
                        await MatchViewModel.Initialize(Players.Where(p => p.IsPlaying).ToList(), Players.Where(p => !p.IsPlaying).ToList());
                        await m_navigation.PushAsync(new MatchPage(MatchViewModel));
                        break;
                }
            }
        }
    }
}