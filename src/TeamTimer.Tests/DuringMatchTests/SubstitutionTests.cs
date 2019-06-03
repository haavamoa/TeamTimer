using System.Collections.Generic;
using FluentAssertions;
using Moq;
using TeamTimer.Models;
using TeamTimer.Services.Dialog.Interfaces;
using TeamTimer.Tests.Helpers;
using TeamTimer.ViewModels;
using TeamTimer.ViewModels.Interfaces.Handlers;
using TeamTimer.ViewModels.Interfaces.ViewModels;
using Xunit;

namespace TeamTimer.Tests.DuringMatchTests
{
    public class SubstitutionTests : TestBase
    {
#pragma warning disable 169
        private IMatchViewModel m_matchViewModel;
#pragma warning restore 169

        [Fact]
        public void Substitute_TwoPlayersAreMarked_ThePlayersShouldChangePositions()
        {
            var playingPlayerToSub = new PlayerBuilder().IsPlaying(true).Build("TestPlayerToSub1");
            var playingPlayers = new List<PlayerViewModel>()
            {
                playingPlayerToSub,
                new PlayerBuilder().IsPlaying(true).Build("TestPlayer2"),
                new PlayerBuilder().IsPlaying(true).Build("TestPlayer3"),
                new PlayerBuilder().IsPlaying(true).Build("TestPlayer4"),
                new PlayerBuilder().IsPlaying(true).Build("TestPlayer5"),
                
            };
            
            var nonPlayingPlayerToSub = new PlayerBuilder().Build("TestPlayerToSub6");
            var nonPlayingPlayers = new List<PlayerViewModel>()
            {
                nonPlayingPlayerToSub,
                new PlayerBuilder().Build("TestPlayer7"),
                new PlayerBuilder().Build("TestPlayer8"),
                new PlayerBuilder().Build("TestPlayer9"),
                new PlayerBuilder().Build("TestPlayer10"),
            };

            m_matchViewModel.Initialize(playingPlayers, nonPlayingPlayers, new Mock<IHandleTeamSetup>().Object);
            
            m_matchViewModel.OnPlayerMarkedForSub(playingPlayerToSub);
            m_matchViewModel.OnPlayerMarkedForSub(nonPlayingPlayerToSub);
            
            m_matchViewModel.PlayingPlayers.Should().Contain(nonPlayingPlayerToSub);
            m_matchViewModel.NonPlayingPlayers.Should().NotContain(nonPlayingPlayerToSub);
            
            m_matchViewModel.NonPlayingPlayers.Should().Contain(playingPlayerToSub);
            m_matchViewModel.PlayingPlayers.Should().NotContain(playingPlayerToSub);

            playingPlayerToSub.IsPlaying.Should().BeFalse();
            nonPlayingPlayerToSub.IsPlaying.Should().BeTrue();
        }
    }
}