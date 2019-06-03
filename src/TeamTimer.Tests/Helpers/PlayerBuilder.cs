using Moq;
using TeamTimer.Models;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces.Handlers;

namespace TeamTimer.Tests.Helpers
{
    public class PlayerBuilder
    {
        private PlayerViewModel m_playerViewModel;
        private bool m_isPlaying;

        public PlayerBuilder IsPlaying(bool isPlaying)
        {
            m_isPlaying = isPlaying;
            return this;
        }
        
        public PlayerViewModel Build(string name)
        {
            m_playerViewModel = new PlayerViewModel(new Player(name), new Mock<IHandleTeamSetup>().Object, new Mock<IHandleMatch>().Object,new 
                Mock<IDialogService>().Object, new Mock<IProfilerService>().Object)
            {
                IsPlaying = m_isPlaying
            };
            return m_playerViewModel;
        }
    }
}