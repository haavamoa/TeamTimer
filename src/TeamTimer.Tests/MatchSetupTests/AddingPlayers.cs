using FluentAssertions;
using LightInject.xUnit2;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.MatchSetupTests
{
    public class MatchSetupTests : TestBase
    {

        private IMainViewModel m_mainViewModel;

        [Fact]
        public void AddPlayerCommand_AddOnePlayer_PlayerCountIsCorrect()
        {
            m_mainViewModel.NewPlayerName = "TestName";
            
            m_mainViewModel.AddPlayerCommand.Execute(null);

            m_mainViewModel.Players.Should().HaveCount(1, "One player has been added");
        }

        [Fact]
        public void AddPlayerCommand_AddMultiplePlayers_PlayerCountIsCorrect()
        {
            m_mainViewModel.NewPlayerName = "TestName1;TestName2;TestName3";

            m_mainViewModel.AddPlayerCommand.Execute(null);

            m_mainViewModel.Players.Should().HaveCount(3, "Three players have been added");
        }
    }
}