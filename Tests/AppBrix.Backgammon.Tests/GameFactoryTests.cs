// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Game;
using AppBrix.Testing;
using System;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Tests;

public class GameFactoryTests : TestsBase<BackgammonModule>
{
    #region Test lifecycle
    protected override void Initialize() => this.App.Start();
    #endregion

    #region Tests
    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreatePlayers()
    {
        var player1Name = "Player 1";
        var player1 = this.App.GetGameFactory().CreatePlayer(player1Name);
        this.Assert(player1.Name == player1Name, "the created player 1 should have the same name as provided");

        var player2Name = "Player 2";
        var player2 = this.App.GetGameFactory().CreatePlayer(player2Name);
        this.Assert(player2.Name == player2Name, "the created player 2 should have the same name as provided");

        this.Assert(player1.Id != player2.Id, "created players should have different ids");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreatePlayerWithId()
    {
        var playerName = "Player 1";
        var playerId = Guid.Parse("9c228435-44e9-4b5c-9221-95d3748d2d01");
        var player = this.App.GetGameFactory().CreatePlayer(playerName, playerId);
        this.Assert(player.Name == playerName, "the created player should have the same name as provided");
        this.Assert(player.Id == playerId, "the created player should have the same id as provided");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestRecreatedPlayer()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var recreated = this.App.GetGameFactory().CreatePlayer(player.Name, player.Id);
        this.Assert(recreated.GetHashCode() == player.GetHashCode(), "recreated player should have the same hash code as the original");
        this.Assert(recreated.Equals(player), "recreated player should be equal to the original");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreatingPlayerWithNullName()
    {
        Action action = () => this.App.GetGameFactory().CreatePlayer(null!);
        this.AssertThrows<ArgumentNullException>(action, "passing a null player name is not allowed");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreatingGame()
    {
        var player1 = this.App.GetGameFactory().CreatePlayer("Player 1");
        var player2 = this.App.GetGameFactory().CreatePlayer("Player 2");
        var game = this.App.GetGameFactory().CreateGame([player1, player2]);
        this.Assert(game.App == this.App, "the app should be the same as the one used when creating the game");
        this.Assert(game.HasEnded == false, "the game has not ended");
        this.Assert(game.HasStarted == false, "the game has not been started");
        this.Assert(game.Turn is null, "the turn should not be set before the game has started");
        this.Assert(string.IsNullOrEmpty(game.Winner), "the game has not ended and there should be no winner yet");
        this.AssertIsInInitialState(game.GetBoard(player1), player1);
        this.AssertIsInInitialState(game.GetBoard(player2), player2);
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreateGameWithNullPlayers()
    {
        Action action = () => this.App.GetGameFactory().CreateGame(null!);
        this.AssertThrows<ArgumentNullException>(action, "passing a null players is not allowed");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreateGameWithOnePlayer()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        Action action = () => this.App.GetGameFactory().CreateGame([player]);
        this.AssertThrows<ArgumentException>(action, "passing only one player is not allowed");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestCreateGameWithThreePlayers()
    {
        var player1 = this.App.GetGameFactory().CreatePlayer("Player 1");
        var player2 = this.App.GetGameFactory().CreatePlayer("Player 2");
        var player3 = this.App.GetGameFactory().CreatePlayer("Player 3");
        Action action = () => this.App.GetGameFactory().CreateGame([player1, player2, player3]);
        this.AssertThrows<ArgumentException>(action, "passing more than 2 players is not allowed");
    }
    #endregion

    #region Private methods
    private void AssertIsInInitialState(IBoard board, IPlayer player)
    {
        this.Assert(board.Bar.Count == 0, "initially there should be no pieces on the board bar");
        this.Assert(board.BearedOff.Count == 0, "initially there should be no beared off pieces");

        this.Assert(board.Lanes[0].Count == 2, "initially lane 0 should have 2 pieces");
        this.Assert(board.Lanes[0].Count(x => x.Player == player.Name) == 2, $"initially pieces on lane 0 should belong to {player.Name}");
        this.Assert(board.Lanes[1].Count == 0, "initially lane 1 should be empty");
        this.Assert(board.Lanes[2].Count == 0, "initially lane 2 should be empty");
        this.Assert(board.Lanes[3].Count == 0, "initially lane 3 should be empty");
        this.Assert(board.Lanes[4].Count == 0, "initially lane 4 should be empty");
        this.Assert(board.Lanes[5].Count == 5, "initially lane 5 should have 5 pieces");
        this.Assert(board.Lanes[5].Count(x => x.Player != player.Name) == 5, $"initially pieces on lane 5 should not belong to {player.Name}");

        this.Assert(board.Lanes[6].Count == 0, "initially lane 6 should be empty");
        this.Assert(board.Lanes[7].Count == 3, "initially lane 7 should have 3 pieces");
        this.Assert(board.Lanes[7].Count(x => x.Player != player.Name) == 3, $"initially pieces on lane 7 should not belong to {player.Name}");
        this.Assert(board.Lanes[8].Count == 0, "initially lane 8 should be empty");
        this.Assert(board.Lanes[9].Count == 0, "initially lane 9 should be empty");
        this.Assert(board.Lanes[10].Count == 0, "initially lane 10 should be empty");
        this.Assert(board.Lanes[11].Count == 5, "initially lane 11 should have 5 pieces");
        this.Assert(board.Lanes[11].Count(x => x.Player == player.Name) == 5, $"initially pieces on lane 11 should belong to {player.Name}");

        this.Assert(board.Lanes[12].Count == 5, "initially lane 12 should have 5 pieces");
        this.Assert(board.Lanes[12].Count(x => x.Player != player.Name) == 5, $"initially pieces on lane 12 should not belong to {player.Name}");
        this.Assert(board.Lanes[13].Count == 0, "initially lane 13 should be empty");
        this.Assert(board.Lanes[14].Count == 0, "initially lane 14 should be empty");
        this.Assert(board.Lanes[15].Count == 0, "initially lane 15 should be empty");
        this.Assert(board.Lanes[16].Count == 3, "initially lane 16 should have 3 pieces");
        this.Assert(board.Lanes[16].Count(x => x.Player == player.Name) == 3, $"initially pieces on lane 16 should belong to {player.Name}");
        this.Assert(board.Lanes[17].Count == 0, "initially lane 17 should be empty");

        this.Assert(board.Lanes[18].Count == 5, "initially lane 18 should have 5 pieces");
        this.Assert(board.Lanes[18].Count(x => x.Player == player.Name) == 5, $"initially pieces on lane 18 should belong to {player.Name}");
        this.Assert(board.Lanes[19].Count == 0, "initially lane 19 should be empty");
        this.Assert(board.Lanes[20].Count == 0, "initially lane 20 should be empty");
        this.Assert(board.Lanes[21].Count == 0, "initially lane 21 should be empty");
        this.Assert(board.Lanes[22].Count == 0, "initially lane 22 should be empty");
        this.Assert(board.Lanes[23].Count == 2, "initially lane 23 should have 2 pieces");
        this.Assert(board.Lanes[23].Count(x => x.Player != player.Name) == 2, $"initially pieces on lane 23 should not belong to {player.Name}");
    }
    #endregion
}
