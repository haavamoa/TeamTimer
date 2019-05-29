using System.Linq;
using FluentAssertions;
using LightInject.xUnit2;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xamarin.Forms.Internals;
using Xunit;

namespace TeamTimer.Tests.MatchSetupTests
{
    public class RemovingPlayers
    {



        [Theory, InjectData]
        public void DeletePlayerCommand_MultiplePlayersAddedAndOneDeleted_PlayersCountIsCorrect(IMainViewModel mainViewModel)
        {
            var playerNameToRemove = "TestName1";
            mainViewModel.NewPlayerName = $"{playerNameToRemove};TestName2;TestName3";

            mainViewModel.AddPlayerCommand.Execute(null);
            mainViewModel.Players.Single(p => p.Name.Equals(playerNameToRemove)).DeletePlayerCommand.Execute(null);

            mainViewModel.Players.Should().HaveCount(2, "One of three players have been deleted");
        }

        [Theory, InjectData]
        public void DeletePlayerCommand_MultiplePlayersAddedAndAllDeleted_PlayersCountIsCorrect(IMainViewModel mainViewModel)
        {
            mainViewModel.NewPlayerName = "TestName1;TestName2;TestName3;TestName4";

            mainViewModel.AddPlayerCommand.Execute(null);
            //ToList() is to make a copy so we can enumerate when removing.
            mainViewModel.Players.ToList().ForEach(p => p.DeletePlayerCommand.Execute(null));

            mainViewModel.Players.Should().BeEmpty("All players have been deleted");
        }
    }
}