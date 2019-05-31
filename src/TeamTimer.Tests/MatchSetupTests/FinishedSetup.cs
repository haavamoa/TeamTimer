using System.Linq;
using FluentAssertions;
using LightInject;
using Moq;
using TeamTimer.Services.Navigation;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.MatchSetupTests
{
    public class FinishedSetup : TestBase
    {
#pragma warning disable 169
        private IMainViewModel m_mainViewModel;
        private Mock<INavigationService> mock_navigationService;
#pragma warning restore 169
        internal override void Configure(IServiceRegistry serviceRegistry)
        {
            mock_navigationService = new Mock<INavigationService>();
            serviceRegistry.Register<INavigationService>(f => mock_navigationService.Object);
        }

        [Fact]
        public void StartCommand_PlayingPlayersIsSet_InitializesAndNavigatesToMatch()
        {
            m_mainViewModel.NewPlayerName = "TestName1;TestName2;TestName3";

            m_mainViewModel.AddPlayerCommand.Execute(null);

            m_mainViewModel.Players.Single(p => p.Name.Equals("TestName1")).IsPlaying = true;

            m_mainViewModel.StartCommand.Execute(null);
            
            mock_navigationService.Verify(n => n.NavigateTo<IMatchViewModel>(), Times.Once);

            m_mainViewModel.MatchViewModel.PlayingPlayers.Should().HaveCount(1);
            m_mainViewModel.MatchViewModel.NonPlayingPlayers.Should().HaveCount(2);
        }

    }
}