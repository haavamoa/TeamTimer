using FluentAssertions;
using LightInject;
using Moq;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.TeamSetupTests
{
    public class MatchSetupTests : TestBase
    {

#pragma warning disable 649
        private ITeamSetupViewModel m_teamSetupViewModel;
        private Mock<IProfilerService> mock_profilerService;
#pragma warning restore 649

        internal override void Configure(IServiceRegistry serviceRegistry)
        {
            mock_profilerService = new Mock<IProfilerService>();
            serviceRegistry.Register<IProfilerService>(p => mock_profilerService.Object);
        }

        [Fact]
        public void AddPlayerCommand_AddOnePlayer_PlayerCountIsCorrect()
        {
            m_teamSetupViewModel.NewPlayerName = "TestName";
            
            m_teamSetupViewModel.AddPlayerCommand.Execute(null);

            m_teamSetupViewModel.Players.Should().HaveCount(1, "One player has been added");
        }

        [Fact]
        public void AddPlayerCommand_AddMultiplePlayers_PlayerCountIsCorrect()
        {
            m_teamSetupViewModel.NewPlayerName = "TestName1;TestName2;TestName3";

            m_teamSetupViewModel.AddPlayerCommand.Execute(null);

            m_teamSetupViewModel.Players.Should().HaveCount(3, "Three players have been added");
            
            mock_profilerService.Verify(p => p.RaiseEvent(It.IsAny<string>()), Times.Once);
        }
    }
}