using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamTimer.Models;
using TeamTimer.Resources.Commands;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces;
using TeamTimer.Views;
using Xamarin.Forms;

namespace TeamTimer.ViewModels
{
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        private readonly IMatchViewModel m_matchViewModel;

        public MainViewModel(IMatchViewModel matchViewModel)
        {
            m_matchViewModel = matchViewModel;
            SaveTeamCommand = new AsyncCommand(_ => SavePlayersAndNavigate());
            AddPlayerCommand = new Command(() => AddPlayer());
            Players = new ObservableCollection<PlayerViewModel>();
        }

        private void AddPlayer()
        {
            var newPlayer = new PlayerViewModel(new Player("EmptyName"));
            Players.Add(newPlayer);
            SelectedPlayer = newPlayer;
        }

        public ObservableCollection<PlayerViewModel> Players { get; }
        public ICommand AddPlayerCommand { get; private set; }

        public INavigation Navigation { get; set; }

        public ICommand SaveTeamCommand { get; private set; }
        
        private PlayerViewModel m_selectedPlayer;

        public PlayerViewModel SelectedPlayer
        {
            get => m_selectedPlayer;
            set => SetProperty(ref m_selectedPlayer, value);
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