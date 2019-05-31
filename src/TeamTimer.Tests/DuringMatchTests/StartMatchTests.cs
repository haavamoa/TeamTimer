using System.Threading.Tasks;
using FluentAssertions;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.DuringMatchTests
{
    public class StartMatchTests : TestBase
    {

#pragma warning disable 169
        private IMatchViewModel m_matchViewModel;
#pragma warning restore 169

        [Fact]
        public void StartMatchCommand_MatchIsNotStarted_MatchShouldBeStarted()
        {
            m_matchViewModel.IsMatchStarted.Should().BeFalse();
            
            m_matchViewModel.StartMatchCommand.Execute(null);

            m_matchViewModel.IsMatchStarted.Should().BeTrue();
        }

        [Fact]
        public async void OnEachMatchSecond_MatchIsStarted_UpdatesDurationOfMatch()
        {
            m_matchViewModel.StartMatchCommand.Execute(null);

            await Task.Delay(3500);

            m_matchViewModel.MatchDuration.Should().Contain("3s");
        }
    }
}