using FluentAssertions;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.MatchSetupTests
{
    public class MatchSetupTests : TestBase
    {

#pragma warning disable 649
        private IMainViewModel m_mainViewModel;
#pragma warning restore 649

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