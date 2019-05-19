using System.Diagnostics.Contracts;
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
        private bool m_isPlaying;

        public PlayerViewModel(Player player)
        {
            m_player = player;
            DeletePlayerCommand = new Command(_ => m_teamHandler?.OnPlayerDeleted(this));
        }


        public string Name => m_player.Name;

        public ICommand DeletePlayerCommand { get; private set; }

        public bool IsPlaying
        {
            get => m_isPlaying;
            set
            {
                SetProperty(ref m_isPlaying, value);
                m_teamHandler?.OnPlayerChanged(this);
            }
        }

        public void Initialize(IHandleTeam teamHandler)
        {
            m_teamHandler = teamHandler;
        }
    }
}