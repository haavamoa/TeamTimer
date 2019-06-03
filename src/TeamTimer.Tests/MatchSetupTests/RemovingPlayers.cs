using System.Linq;
using FluentAssertions;
using LightInject;
using Moq;
using TeamTimer.Services.Profiling;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms.Internals;
using Xunit;

namespace TeamTimer.Tests.MatchSetupTests
{
    public class RemovingPlayers : TestBase
    {

#pragma warning disable 649
        private IMainViewModel m_mainViewModel;
        private Mock<IProfilerService> mock_profilerService;
#pragma warning restore 649

        
        internal override void Configure(IServiceRegistry serviceRegistry)
        {
            mock_profilerService = new Mock<IProfilerService>();
            serviceRegistry.Register<IProfilerService>(p => mock_profilerService.Object);
        }
        
        [Fact]
        public void DeletePlayerCommand_MultiplePlayersAddedAndOneDeleted_PlayersCountIsCorrect()
        {
            var playerNameToRemove = "TestName1";
            m_mainViewModel.NewPlayerName = $"{playerNameToRemove};TestName2;TestName3";

            m_mainViewModel.AddPlayerCommand.Execute(null);
            m_mainViewModel.Players.Single(p => p.Name.Equals(playerNameToRemove)).DeletePlayerCommand.Execute(null);

            m_mainViewModel.Players.Should().HaveCount(2, "One of three players have been deleted");
        }

        [Fact]
        public void DeletePlayerCommand_MultiplePlayersAddedAndAllDeleted_PlayersCountIsCorrect()
        {
            m_mainViewModel.NewPlayerName = "TestName1;TestName2;TestName3;TestName4";

            m_mainViewModel.AddPlayerCommand.Execute(null);
            //ToList() is to make a copy so we can enumerate when removing.
            m_mainViewModel.Players.ToList().ForEach(p => p.DeletePlayerCommand.Execute(null));

            m_mainViewModel.Players.Should().BeEmpty("All players have been deleted");
        }
    }
}