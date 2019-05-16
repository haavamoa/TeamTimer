using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace TeamTimer.ViewModels.Interfaces
{
    public interface IMainViewModel
    {
        INavigation Navigation { get; set; }
        ObservableCollection<PlayerViewModel> Players { get; }
        ICommand AddPlayerCommand { get; }
        ICommand SaveTeamCommand { get; }
        PlayerViewModel SelectedPlayer { get; set; }
    }
}