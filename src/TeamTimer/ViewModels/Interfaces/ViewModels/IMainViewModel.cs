using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IMainViewModel : IViewModel
    {
        ObservableCollection<PlayerViewModel> Players { get; }
        ICommand AddPlayerCommand { get; }
        ICommand StartCommand { get; }
        string NewPlayerName { get; set; }
        int NumberOfStartingPlayers { get; }
        IMatchViewModel MatchViewModel { get; }
        ObservableCollection<PlayerViewModel>? SelectedItems { get; }
    }
}