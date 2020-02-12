// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Game;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Tests
{
    public class GameFactoryTests : IDisposable
    {
        #region Setup and cleanup
        public GameFactoryTests()
        {
            this.app = TestUtils.CreateTestApp(typeof(BackgammonModule));
            this.app.Start();
        }

        public void Dispose()
        {
            this.app.Stop();
        }
        #endregion

        #region Tests
        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
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

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreatePlayerWithId()
        {
            var playerName = "Player 1";
            var playerId = Guid.Parse("9c228435-44e9-4b5c-9221-95d3748d2d01");
            var player = this.app.GetGameFactory().CreatePlayer(playerName, playerId);
            player.Name.Should().Be(playerName, "the created player should have the same name as provided");
            player.Id.Should().Be(playerId, "the created player should have the same id as provided");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestRecreatedPlayer()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var recreated = this.app.GetGameFactory().CreatePlayer(player.Name, player.Id);
            recreated.GetHashCode().Should().Be(player.GetHashCode(), "recreated player should have the same hash code as the original");
            recreated.Should().Be(player, "recreated player should be equal to the original");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreatingPlayerWithNullName()
        {
            Action action = () => this.app.GetGameFactory().CreatePlayer(null);
            action.Should().Throw<ArgumentNullException>("passing a null player name is not allowed");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreatingGame()
        {
            var player1 = this.app.GetGameFactory().CreatePlayer("Player 1");
            var player2 = this.app.GetGameFactory().CreatePlayer("Player 2");
            var game = this.app.GetGameFactory().CreateGame(new[] { player1, player2 });
            game.App.Should().Be(this.app, "the app should be the same as the one used when creating the game");
            game.HasEnded.Should().BeFalse("the game has not ended");
            game.HasStarted.Should().BeFalse("the game has not been started");
            game.Turn.Should().BeNull("the turn should not be set before the game has started");
            game.Winner.Should().BeNull("the game has not ended and there should be no winner yet");
            this.AssertIsInInitialState(game.GetBoard(player1), player1);
            this.AssertIsInInitialState(game.GetBoard(player2), player2);
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreateGameWithNullPlayers()
        {
            Action action = () => this.app.GetGameFactory().CreateGame(null);
            action.Should().Throw<ArgumentNullException>("passing a null players is not allowed");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreateGameWithOnePlayer()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            Action action = () => this.app.GetGameFactory().CreateGame(new[] { player });
            action.Should().Throw<ArgumentException>("passing only one player is not allowed");
        }

        [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
        public void TestCreateGameWithThreePlayers()
        {
            var player1 = this.app.GetGameFactory().CreatePlayer("Player 1");
            var player2 = this.app.GetGameFactory().CreatePlayer("Player 2");
            var player3 = this.app.GetGameFactory().CreatePlayer("Player 3");
            Action action = () => this.app.GetGameFactory().CreateGame(new[] { player1, player2, player3 });
            action.Should().Throw<ArgumentException>("passing more than 2 players is not allowed");
        }
        #endregion

        #region Private methods
        private void AssertIsInInitialState(IBoard board, IPlayer player)
        {
            board.Bar.Should().BeEmpty("initially there should be no pieces on the board bar");
            board.BearedOff.Should().BeEmpty("initially there should be no beared off pieces");

            board.Lanes[0].Count.Should().Be(2, "initially lane 0 should have 2 pieces");
            board.Lanes[0].Count(x => x.Player == player.Name).Should().Be(2, "initially pieces on lane 0 should belong to " + player.Name);
            board.Lanes[1].Should().BeEmpty("initially lane 1 should be empty");
            board.Lanes[2].Should().BeEmpty("initially lane 2 should be empty");
            board.Lanes[3].Should().BeEmpty("initially lane 3 should be empty");
            board.Lanes[4].Should().BeEmpty("initially lane 4 should be empty");
            board.Lanes[5].Count.Should().Be(5, "initially lane 5 should have 5 pieces");
            board.Lanes[5].Count(x => x.Player != player.Name).Should().Be(5, "initially pieces on lane 5 should not belong to " + player.Name);

            board.Lanes[6].Should().BeEmpty("initially lane 6 should be empty");
            board.Lanes[7].Count.Should().Be(3, "initially lane 7 should have 3 pieces");
            board.Lanes[7].Count(x => x.Player != player.Name).Should().Be(3, "initially pieces on lane 7 should not belong to " + player.Name);
            board.Lanes[8].Should().BeEmpty("initially lane 8 should be empty");
            board.Lanes[9].Should().BeEmpty("initially lane 9 should be empty");
            board.Lanes[10].Should().BeEmpty("initially lane 10 should be empty");
            board.Lanes[11].Count.Should().Be(5, "initially lane 11 should have 5 pieces");
            board.Lanes[11].Count(x => x.Player == player.Name).Should().Be(5, "initially pieces on lane 11 should belong to " + player.Name);

            board.Lanes[12].Count.Should().Be(5, "initially lane 12 should have 5 pieces");
            board.Lanes[12].Count(x => x.Player != player.Name).Should().Be(5, "initially pieces on lane 12 should not belong to " + player.Name);
            board.Lanes[13].Should().BeEmpty("initially lane 13 should be empty");
            board.Lanes[14].Should().BeEmpty("initially lane 14 should be empty");
            board.Lanes[15].Should().BeEmpty("initially lane 15 should be empty");
            board.Lanes[16].Count.Should().Be(3, "initially lane 16 should have 3 pieces");
            board.Lanes[16].Count(x => x.Player == player.Name).Should().Be(3, "initially pieces on lane 16 should belong to " + player.Name);
            board.Lanes[17].Should().BeEmpty("initially lane 17 should be empty");

            board.Lanes[18].Count.Should().Be(5, "initially lane 18 should have 5 pieces");
            board.Lanes[18].Count(x => x.Player == player.Name).Should().Be(5, "initially pieces on lane 18 should belong to " + player.Name);
            board.Lanes[19].Should().BeEmpty("initially lane 19 should be empty");
            board.Lanes[20].Should().BeEmpty("initially lane 20 should be empty");
            board.Lanes[21].Should().BeEmpty("initially lane 21 should be empty");
            board.Lanes[22].Should().BeEmpty("initially lane 22 should be empty");
            board.Lanes[23].Count.Should().Be(2, "initially lane 23 should have 2 pieces");
            board.Lanes[23].Count(x => x.Player != player.Name).Should().Be(2, "initially pieces on lane 23 should not belong to " + player.Name);
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
