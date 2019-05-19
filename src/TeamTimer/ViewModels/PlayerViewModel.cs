using System.Windows.Input;
using TeamTimer.Models;
using TeamTimer.ViewModels.Base;
using TeamTimer.ViewModels.Interfaces.Handlers;
using Xamarin.Forms;

namespace TeamTimer.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private readonly Player m_player;
        private IHandleTeam? m_teamHandler;

        public PlayerViewModel(Player player)
        {
            m_player = player;
            DeletePlayerCommand = new Command(_ => m_teamHandler?.OnPlayerDeleted(this));
        }
        

        public string Name
        {
            get => m_player.Name;
            set
            {
                m_player.Name = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeletePlayerCommand { get; private set; }

        public void Initialize(IHandleTeam teamHandler)
        {
            m_teamHandler = teamHandler;
        }
    }
}