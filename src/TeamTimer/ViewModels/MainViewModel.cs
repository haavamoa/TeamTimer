using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DLToolkit.Forms.Controls.Helpers.FlowListView;
using TeamTimer.Models;
using TeamTimer.Resources.Commands;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Navigation;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.Handlers;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using TeamTimer.Views;
using Xamarin.Forms;

namespace TeamTimer.ViewModels
{
    [Preserve(AllMembers = true)]
    public class MainViewModel : BaseViewModel, IMainViewModel, IHandleTeamSetup
    {
        private readonly IDialogService m_dialogService;
        private readonly INavigationService m_navigationService;
        private string m_newPlayerName = string.Empty;

        public MainViewModel(IMatchViewModel matchViewModel, IDialogService dialogService, INavigationService navigationService)
        {
            MatchViewModel = matchViewModel;
            m_dialogService = dialogService;
            m_navigationService = navigationService;
            StartCommand = new AsyncCommand(_ => NavigateToMatch(), _ => Players.Any(p => p.IsPlaying));
            AddPlayerCommand = new Command(AddPlayerOrPlayers, () => !string.IsNullOrEmpty(NewPlayerName));
            Players = new ObservableCollection<PlayerViewModel>();
        }


        public void OnPlayerDeleted(PlayerViewModel deletedPlayer)
        {
            Players.Remove(deletedPlayer);
            OnPropertyChanged(nameof(NumberOfStartingPlayers));
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)StartCommand).ChangeCanExecute();
        }

        public void OnPlayerChanged(PlayerViewModel changedPlayer)
        {
            OnPropertyChanged(nameof(NumberOfStartingPlayers));
            if (changedPlayer.IsPlaying)
            {
                SelectedItems?.Add(changedPlayer);
            }
            else
            {
                SelectedItems?.Remove(changedPlayer);
            }
            OnPropertyChanged(nameof(SelectedItems));
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)StartCommand).ChangeCanExecute();
        }

        public ObservableCollection<PlayerViewModel> Players { get; private set; }

        public ICommand AddPlayerCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public string NewPlayerName
        {
            get => m_newPlayerName;
            set => SetProperty(ref m_newPlayerName, value, commandsToChangeCanExecute: (Command)AddPlayerCommand);
        }

        public int NumberOfStartingPlayers => Players.Count(p => p.IsPlaying);

        public IMatchViewModel MatchViewModel { get; }
        public ObservableCollection<PlayerViewModel> SelectedItems { get; }

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

            Players = new ObservableCollection<PlayerViewModel>(Players.OrderBy(p => p.Name, StringComparer.CurrentCulture));
            OnPropertyChanged(nameof(Players));

            OnPropertyChanged(nameof(NumberOfStartingPlayers));
        }

        private void AddPlayer(string newPlayerName)
        {
            var newPlayer = new PlayerViewModel(new Player(newPlayerName), this, MatchViewModel, m_dialogService);
            Players.Add(newPlayer);
            OnPropertyChanged(nameof(Players));
            ((AsyncCommand)StartCommand).ChangeCanExecute();
        }

        private bool IsMultipleNames(string newPlayerName, out List<PlayerViewModel> players)
        {
            var newPlayerNames = newPlayerName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            players = new List<PlayerViewModel>();
            players.AddRange(newPlayerNames.Select(newPlayer => new PlayerViewModel(new Player(newPlayer), this, MatchViewModel, m_dialogService)));
            return players.Count > 1;
        }

        private async Task NavigateToMatch()
        {
            await MatchViewModel.Initialize(Players.Where(p => p.IsPlaying).ToList(), Players.Where(p => !p.IsPlaying).ToList(), this);
            await m_navigationService.NavigateTo<IMatchViewModel>();
        }

        public void Dispose()
        {
            
        }

        public string Title => "Setup Team";
        public bool IsBusy { get; set; }
    }
}