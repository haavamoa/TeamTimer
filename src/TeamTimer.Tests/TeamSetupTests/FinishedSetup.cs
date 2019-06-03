using System.Linq;
using FluentAssertions;
using LightInject;
using Moq;
using TeamTimer.Services.Navigation;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.TeamSetupTests
{
    public class FinishedSetup : TestBase
    {

#pragma warning disable 649
        private ITeamSetupViewModel m_teamSetupViewModel;
#pragma warning restore 649
        private Mock<INavigationService> mock_navigationService;
        private Mock<IProfilerService> mock_profilerService;

        internal override void Configure(IServiceRegistry serviceRegistry)
        {
            mock_navigationService = new Mock<INavigationService>();
            mock_profilerService = new Mock<IProfilerService>();
            serviceRegistry.Register<INavigationService>(f => mock_navigationService.Object);
            serviceRegistry.Register<IProfilerService>(p => mock_profilerService.Object);
        }

        [Fact]
        public void StartCommand_PlayingPlayersIsSet_InitializesAndNavigatesToMatch()
        {
            m_teamSetupViewModel.NewPlayerName = "TestName1;TestName2;TestName3";

            m_teamSetupViewModel.AddPlayerCommand.Execute(null);

            m_teamSetupViewModel.Players.Single(p => p.Name.Equals("TestName1")).IsPlaying = true;

            m_teamSetupViewModel.StartCommand.Execute(null);
            
            mock_navigationService.Verify(n => n.NavigateTo<IMatchViewModel>(), Times.Once);

            m_teamSetupViewModel.MatchViewModel.PlayingPlayers.Should().HaveCount(1);
            m_teamSetupViewModel.MatchViewModel.NonPlayingPlayers.Should().HaveCount(2);
        }

    }
}