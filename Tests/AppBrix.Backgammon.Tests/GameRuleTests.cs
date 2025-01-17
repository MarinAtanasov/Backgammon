// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using AppBrix.Backgammon.Game.Impl;
using AppBrix.Backgammon.Rules.Strategies;
using AppBrix.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Tests;

public class GameRuleTests : TestsBase<BackgammonModule>
{
    #region Test lifecycle
    protected override void Initialize() => this.App.Start();
    #endregion

    #region Tests
    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestEnterPieceStrategyZeroDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Bar.Add(new DefaultPiece(player));
        board.Lanes[5].Add(new DefaultPiece(enemy));
        board.Lanes[5].Add(new DefaultPiece(enemy));
        var strategy = new EnterPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone, "the strategy should short-circuit the chain if there are pieces to enter");
        this.Assert(moves.Count == 0, "there should be no available moves because one dice is used and other position is taken");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestEnterPieceStrategyOneDie()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Bar.Add(new DefaultPiece(player));
        var strategy = new EnterPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone, "the strategy should short-circuit the chain if there are pieces to enter");
        this.Assert(moves.Count == 1, "there should be one available move because only one dice is unused");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[0]), "die with value 6 is unused");
        this.Assert(moves[0].Lane == board.Bar, "the piece on the bar needs to be entered");
        this.Assert(moves[0].LaneIndex == -1, "the bar's index should be -1");

        var piece = board.Bar[0];
        strategy.MovePiece(player, board, moves[0]);
        this.Assert(board.Bar.Count == 0, "the piece should have been moved from the bar");
        this.Assert(board.Lanes[5].Count == 1, "the piece should have been moved to lane 5");
        this.Assert(board.Lanes[5][0] == piece, "the moved piece should be the same as the removed one");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestEnterPieceStrategyTwoDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(false, 5), new DefaultDie(false, 4), new DefaultDie(true, 3) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Bar.Add(new DefaultPiece(player));
        board.Lanes[4].Add(new DefaultPiece(enemy));
        board.Lanes[3].Add(new DefaultPiece(enemy));
        board.Lanes[3].Add(new DefaultPiece(enemy));
        var strategy = new EnterPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone, "the strategy should short-circuit the chain if there are pieces to enter");
        this.Assert(moves.Count == 2, "there should be two available moves");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[0]), "die with value 6 is unused");
        this.Assert(moves[0].Lane == board.Bar, "the piece on the bar needs to be entered");
        this.Assert(moves[0].LaneIndex == -1, "the bar's index should be -1");
        this.Assert(object.ReferenceEquals(moves[1].Die, dice[1]), "die with value 5 is unused and the target has only 1 enemy piece");
        this.Assert(moves[1].Lane == board.Bar, "the piece on the bar needs to be entered");
        this.Assert(moves[1].LaneIndex == -1, "the bar's index should be -1");

        var piece = board.Bar[0];
        var enemyPiece = board.Lanes[4][0];
        strategy.MovePiece(player, board, moves[1]);
        this.Assert(board.Bar.Count == 1, "the enemy piece should have been moved to the bar");
        this.Assert(board.Bar[0] == enemyPiece, "the enemy piece should be the same as the moved one");
        this.Assert(board.Lanes[4].Count == 1, "the piece should have been moved to lane 5");
        this.Assert(board.Lanes[4][0] == piece, "the moved piece should be the same as the removed one");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestEnterPieceStrategyNoBarredPieces()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Bar.Add(new DefaultPiece(enemy));
        board.Lanes[0].Add(new DefaultPiece(player));
        var strategy = new EnterPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should not short-circuit the chain if there are no pieces to enter");
        this.Assert(moves.Count == 0, "there should be no available moves yet because no pieces need to enter");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestMovePieceStrategyZeroDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(true, 2), new DefaultDie(false, 1) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[0].Add(new DefaultPiece(player));
        board.Lanes[0].Add(new DefaultPiece(player));
        board.Lanes[1].Add(new DefaultPiece(enemy));
        board.Lanes[1].Add(new DefaultPiece(enemy));
        var strategy = new MovePieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 0, "there should be no moves available");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestMovePieceStrategyOneDie()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 2), new DefaultDie(false, 1) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[0].Add(new DefaultPiece(player));
        board.Lanes[0].Add(new DefaultPiece(player));
        board.Lanes[1].Add(new DefaultPiece(enemy));
        board.Lanes[1].Add(new DefaultPiece(enemy));
        var strategy = new MovePieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 1, "there should be one available move because only one dice is unused");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[0]), "only die with value 2 is usable");
        this.Assert(moves[0].Lane == board.Lanes[0], "there is only one piece for this player");
        this.Assert(moves[0].LaneIndex == 0, "the player's only piece is on that lane");

        var piece = board.Lanes[0][1];
        strategy.MovePiece(player, board, moves[0]);
        this.Assert(board.Lanes[0].Count == 1, "the other piece should not have been moved");
        this.Assert(board.Lanes[2].Count == 1, "the piece should have been moved to this lane");
        this.Assert(board.Lanes[2][0] == piece, "the moved piece should be the same as the selected one");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestMovePieceStrategyTwoDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 4), new DefaultDie(true, 3), new DefaultDie(false, 2), new DefaultDie(false, 1) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[20].Add(new DefaultPiece(player));
        board.Lanes[21].Add(new DefaultPiece(enemy));
        var strategy = new MovePieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 2, "there should be two available moves");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[2]), "die with value 2 is unused");
        this.Assert(moves[0].Lane == board.Lanes[20], "there is only one piece for this player");
        this.Assert(moves[0].LaneIndex == 20, "the player's only piece is on that lane");
        this.Assert(object.ReferenceEquals(moves[1].Die, dice[3]), "die with value 1 is unused and the target has only 1 enemy piece");
        this.Assert(moves[1].Lane == board.Lanes[20], "there is only one piece for this player");
        this.Assert(moves[1].LaneIndex == 20, "the player's only piece is on that lane");

        var piece = board.Lanes[20][0];
        var enemyPiece = board.Lanes[21][0];
        strategy.MovePiece(player, board, moves[1]);
        this.Assert(board.Bar.Count == 1, "the enemy piece should have been moved to the bar");
        this.Assert(board.Bar[0] == enemyPiece, "the enemy piece should be the same as the removed one");
        this.Assert(board.Lanes[21].Count == 1, "the piece should have been moved to this lane");
        this.Assert(board.Lanes[21][0] == piece, "the moved piece should be the same as the selected one");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestBearOffPieceStrategyZeroDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(false, 5) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[17].Add(new DefaultPiece(player));
        board.Lanes[18].Add(new DefaultPiece(player));
        board.Lanes[19].Add(new DefaultPiece(enemy));
        var strategy = new BearOffPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 0, "there should be no moves available");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestBearOffPieceStrategyOneDie()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(true, 6), new DefaultDie(false, 5), new DefaultDie(false, 4), new DefaultDie(false, 3) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[19].Add(new DefaultPiece(player));
        board.Lanes[20].Add(new DefaultPiece(enemy));
        var strategy = new BearOffPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 1, "there should be only one move available");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[1]), "this is the only available die which can be used to exit");
        this.Assert(moves[0].Lane == board.Lanes[19], "this lane contains the only piece for the player");
        this.Assert(moves[0].LaneIndex == 19, "this lane contains the only piece for the player");

        var piece = board.Lanes[19][0];
        strategy.MovePiece(player, board, moves[0]);
        this.Assert(board.Lanes[19].Count == 0, "the piece should have been beared off");
        this.Assert(board.BearedOff.Count == 1, "the piece should have been moved to this lane");
        this.Assert(board.BearedOff[0] == piece, "the beared off piece should be the same as the selected one");
    }

    [Fact, Trait(TestCategories.Category, TestCategories.Functional)]
    public void TestBearOffPieceStrategyTwoDice()
    {
        var player = this.App.GetGameFactory().CreatePlayer("Player 1");
        var enemy = this.App.GetGameFactory().CreatePlayer("Player 2");
        var dice = new[] { new DefaultDie(false, 6), new DefaultDie(false, 5), new DefaultDie(false, 4), new DefaultDie(false, 3) };
        var turn = new DefaultTurn(player, dice);
        var context = new DefaultGameRuleStrategyContext();
        var board = this.CreateDefaultBoard();
        board.Lanes[19].Add(new DefaultPiece(player));
        board.Lanes[19].Add(new DefaultPiece(player));
        board.Lanes[20].Add(new DefaultPiece(enemy));
        var strategy = new BearOffPieceStrategy();

        var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
        this.Assert(context.IsDone == false, "the strategy should never short-circuit");
        this.Assert(moves.Count == 2, "there should be two move available");
        this.Assert(object.ReferenceEquals(moves[0].Die, dice[0]), "a larger move can be used when there is no piece before ours");
        this.Assert(moves[0].Lane == board.Lanes[19], "this lane contains the only piece for the player");
        this.Assert(moves[0].LaneIndex == 19, "this lane contains the only piece for the player");
        this.Assert(object.ReferenceEquals(moves[1].Die, dice[1]), "an exact die can be used to bear off the piece");
        this.Assert(moves[1].Lane == board.Lanes[19], "this lane contains the only piece for the player");
        this.Assert(moves[1].LaneIndex == 19, "this lane contains the only piece for the player");

        var unmovedPiece = board.Lanes[19][0];
        var piece = board.Lanes[19][1];
        strategy.MovePiece(player, board, moves[0]);
        this.Assert(board.Lanes[19].Count == 1, "only one piece should be beared off");
        this.Assert(board.Lanes[19][0] == unmovedPiece, "the remaining piece should not be changed");
        this.Assert(board.BearedOff.Count == 1, "the piece should have been moved to this lane");
        this.Assert(board.BearedOff[0] == piece, "the beared off piece should be the same as the selected one");
    }
    #endregion

    #region Private methods
    private IList<IGameMove> CallGetStrategyValidMoves(GameRuleStrategyBase strategy, IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
    {
        return strategy.GetStrategyAvailableMoves(board, turn, context)
            .Cast<IGameMove>()
            .ToList();
    }

    private IGameBoard CreateDefaultBoard()
    {
        return new DefaultBoard();
    }
    #endregion
}
