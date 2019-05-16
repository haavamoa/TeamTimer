using TeamTimer.Models;
using TeamTimer.ViewModels.Base;

namespace TeamTimer.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        private readonly Player m_player;

        public PlayerViewModel(Player player)
        {
            m_player = player;
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
    }
}