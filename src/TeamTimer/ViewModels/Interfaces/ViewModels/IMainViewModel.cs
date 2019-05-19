using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TeamTimer.ViewModels.Interfaces.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<PlayerViewModel> Players { get; }
        ICommand AddPlayerCommand { get; }
        ICommand SaveTeamCommand { get; }
        string NewPlayerName { get; set; }
        Task Initialize(INavigation navigation);
    }
}