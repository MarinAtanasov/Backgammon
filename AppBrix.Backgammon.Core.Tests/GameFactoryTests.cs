// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Game;
using AppBrix.Events;
using AppBrix.Factory;
using AppBrix.Resolver;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Core.Tests
{
    public class GameFactoryTests
    {
        #region Setup and cleanup
        public GameFactoryTests()
        {
            this.app = TestUtils.CreateTestApp(
                typeof(ResolverModule),
                typeof(FactoryModule),
                typeof(EventsModule),
                typeof(CoreModule));
            this.app.Start();
        }

        public void Dispose()
        {
            this.app.Stop();
        }
        #endregion

        #region Tests
        [Fact]
        public void TestCreatePlayers()
        {
            var player1Name = "Player 1";
            var player1 = this.app.GetGameFactory().CreatePlayer(player1Name);
            player1.Name.Should().Be(player1Name, "the created player 1 should have the same name as provided");

            var player2Name = "Player 2";
            var player2 = this.app.GetGameFactory().CreatePlayer(player2Name);
            player2.Name.Should().Be(player2Name, "the created player 2 should have the same name as provided");

            player1.Id.Should().NotBe(player2.Id, "created players should have different ids");
        }

        [Fact]
        public void TestCreatePlayerWithId()
        {
            var playerName = "Player 1";
            var playerId = Guid.Parse("9c228435-44e9-4b5c-9221-95d3748d2d01");
            var player = this.app.GetGameFactory().CreatePlayer(playerName, playerId);
            player.Name.Should().Be(playerName, "the created player should have the same name as provided");
            player.Id.Should().Be(playerId, "the created player should have the same id as provided");
        }

        [Fact]
        public void TestRecreatedPlayer()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var recreated = this.app.GetGameFactory().CreatePlayer(player.Name, player.Id);
            recreated.GetHashCode().Should().Be(player.GetHashCode(), "recreated player should have the same hash code as the original");
            recreated.Should().Be(player, "recreated player should be equal to the original");
        }

        [Fact]
        public void TestCreatingPlayerWithNullName()
        {
            Action action = () => this.app.GetGameFactory().CreatePlayer(null);
            action.ShouldThrow<ArgumentNullException>("passing a null player name is not allowed");
        }

        [Fact]
        public void TestCreatingGame()
        {
            var player1 = this.app.GetGameFactory().CreatePlayer("Player 1");
            var player2 = this.app.GetGameFactory().CreatePlayer("Player 2");
            var game = this.app.GetGameFactory().CreateGame(new[] { player1, player2 });
            game.AllowedMoves.Should().NotBeNull("allowed moves should not be null even before starting the game");
            game.AllowedMoves.Count.Should().Be(0, "there should be no allowed moves before starting the game");
            game.App.Should().Be(this.app, "the app should be the same as the one used when creating the game");
            game.HasEnded.Should().BeFalse("the game has not ended");
            game.HasStarted.Should().BeFalse("the game has not been started");
            game.Turn.Should().BeNull("the turn should not be set before the game has started");
            game.Winner.Should().BeNull("the game has not ended and there should be no winner yet");
        }

        [Fact]
        public void TestCreateGameWithNullPlayers()
        {
            Action action = () => this.app.GetGameFactory().CreateGame(null);
            action.ShouldThrow<ArgumentNullException>("passing a null players is not allowed");
        }

        [Fact]
        public void TestCreateGameWithOnePlayer()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            Action action = () => this.app.GetGameFactory().CreateGame(new[] { player });
            action.ShouldThrow<ArgumentException>("passing only one player is not allowed");
        }

        [Fact]
        public void TestCreateGameWithThreePlayers()
        {
            var player1 = this.app.GetGameFactory().CreatePlayer("Player 1");
            var player2 = this.app.GetGameFactory().CreatePlayer("Player 2");
            var player3 = this.app.GetGameFactory().CreatePlayer("Player 3");
            Action action = () => this.app.GetGameFactory().CreateGame(new[] { player1, player2, player3 });
            action.ShouldThrow<ArgumentException>("passing more than 2 players is not allowed");
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
