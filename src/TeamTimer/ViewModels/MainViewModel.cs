using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly IMatchViewModel m_matchViewModel;
        private string m_newPlayerName;

        public MainViewModel(IMatchViewModel matchViewModel)
        {
            m_matchViewModel = matchViewModel;
            SaveTeamCommand = new AsyncCommand(_ => SavePlayersAndNavigate());
            AddPlayerCommand = new Command(AddPlayerOrPlayers, () => !string.IsNullOrEmpty(NewPlayerName));
            Players = new ObservableCollection<PlayerViewModel>();
        }

        public void OnPlayerDeleted(PlayerViewModel deletedPlayer) => Players.Remove(deletedPlayer);

        public ObservableCollection<PlayerViewModel> Players { get; }

        public ICommand AddPlayerCommand { get; private set; }

        public INavigation Navigation { get; set; }

        public ICommand SaveTeamCommand { get; private set; }

        public string NewPlayerName
        {
            get => m_newPlayerName;
            set => SetProperty(ref m_newPlayerName, value, commandsToChangeCanExecute: (Command)AddPlayerCommand);
        }

        public async Task Initialize()
        {
            await m_matchViewModel.Initialize();
        }

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
        }

        private void AddPlayer(string newPlayerName)
        {
            var newPlayer = new PlayerViewModel(new Player(newPlayerName));
            newPlayer.Initialize(this as IHandleTeam);
            Players.Add(newPlayer);
        }

        private bool IsMultipleNames(string newPlayerName, out List<PlayerViewModel> players)
        {
            var newPlayerNames = newPlayerName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            players = new List<PlayerViewModel>();
            players.AddRange(newPlayerNames.Select(newplayer => new PlayerViewModel(new Player(newplayer))));
            return players.Any();
        }

        private async Task SavePlayersAndNavigate()
        {
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            switch (currentPage)
            {
                case null:
                    await Navigation.PushAsync(new MainPage(this));
                    break;
                case MainPage _:
                    await Navigation.PushAsync(new MatchPage(m_matchViewModel));
                    break;
            }
        }
    }
}