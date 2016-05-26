// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using AppBrix.Backgammon.Game.Impl;
using AppBrix.Backgammon.Rules.Strategies;
using AppBrix.Container;
using AppBrix.Events;
using AppBrix.Factory;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Tests
{
    public class GameRuleTests
    {
        #region Setup and cleanup
        public GameRuleTests()
        {
            this.app = TestUtils.CreateTestApp(
                typeof(ContainerModule),
                typeof(FactoryModule),
                typeof(EventsModule),
                typeof(BackgammonModule));
            this.app.Start();
        }

        public void Dispose()
        {
            this.app.Stop();
        }
        #endregion

        #region Tests
        [Fact]
        public void TestEnterPieceStrategyZeroDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Bar.Add(new DefaultPiece(player));
            board.Lanes[5].Add(new DefaultPiece(enemy));
            board.Lanes[5].Add(new DefaultPiece(enemy));
            var strategy = new EnterPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are pieces to enter");
            moves.Should().BeEmpty("there should be no available moves because one dice is used and other position is taken");
        }

        [Fact]
        public void TestEnterPieceStrategyOneDie()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Bar.Add(new DefaultPiece(player));
            var strategy = new EnterPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are pieces to enter");
            moves.Should().ContainSingle("there should be one available move because only one dice is unused");
            moves[0].Die.Should().BeSameAs(dice[0], "die with value 6 is unused");
            moves[0].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            moves[0].LaneIndex.Should().Be(-1, "the bar's index should be -1");

            var piece = board.Bar[0];
            strategy.MovePiece(player, board, moves[0]);
            board.Bar.Should().BeEmpty("the piece should have been moved from the bar");
            board.Lanes[5].Should().ContainSingle("the piece should have been moved to lane 5");
            board.Lanes[5][0].Should().BeSameAs(piece, "the moved piece should be the same as the removed one");
        }

        [Fact]
        public void TestEnterPieceStrategyTwoDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
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
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are pieces to enter");
            moves.Count.Should().Be(2, "there should be two available moves");
            moves[0].Die.Should().BeSameAs(dice[0], "die with value 6 is unused");
            moves[0].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            moves[0].LaneIndex.Should().Be(-1, "the bar's index should be -1");
            moves[1].Die.Should().BeSameAs(dice[1], "die with value 5 is unused and the target has only 1 enemy piece");
            moves[1].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            moves[1].LaneIndex.Should().Be(-1, "the bar's index should be -1");

            var piece = board.Bar[0];
            var enemyPiece = board.Lanes[4][0];
            strategy.MovePiece(player, board, moves[1]);
            board.Bar.Should().ContainSingle("the enemy piece should have been moved to the bar");
            board.Bar[0].Should().BeSameAs(enemyPiece, "the enemy piece should be the same as the moved one");
            board.Lanes[4].Should().ContainSingle("the piece should have been moved to lane 5");
            board.Lanes[4][0].Should().BeSameAs(piece, "the moved piece should be the same as the removed one");
        }

        [Fact]
        public void TestEnterPieceStrategyNoBarredPieces()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(false, 6), new DefaultDie(true, 5) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Bar.Add(new DefaultPiece(enemy));
            board.Lanes[0].Add(new DefaultPiece(player));
            var strategy = new EnterPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeFalse("the strategy should not short-circuit the chain if there are no pieces to enter");
            moves.Should().BeEmpty("there should be no available moves yet because no pieces need to enter");
        }

        [Fact]
        public void TestMovePieceStrategyZeroDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
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
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Should().BeEmpty("there should be no moves available");
        }

        [Fact]
        public void TestMovePieceStrategyOneDie()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
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
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Should().ContainSingle("there should be one available move because only one dice is unused");
            moves[0].Die.Should().BeSameAs(dice[0], "only die with value 2 is usable");
            moves[0].Lane.Should().BeSameAs(board.Lanes[0], "there is only one piece for this player");
            moves[0].LaneIndex.Should().Be(0, "the player's only piece is on that lane");

            var piece = board.Lanes[0][1];
            strategy.MovePiece(player, board, moves[0]);
            board.Lanes[0].Should().ContainSingle("the other piece should not have been moved");
            board.Lanes[2].Should().ContainSingle("the piece should have been moved to this lane");
            board.Lanes[2][0].Should().BeSameAs(piece, "the moved piece should be the same as the selected one");
        }

        [Fact]
        public void TestMovePieceStrategyTwoDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(false, 4), new DefaultDie(true, 3), new DefaultDie(false, 2), new DefaultDie(false, 1) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Lanes[20].Add(new DefaultPiece(player));
            board.Lanes[21].Add(new DefaultPiece(enemy));
            var strategy = new MovePieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Count.Should().Be(2, "there should be two available moves");
            moves[0].Die.Should().BeSameAs(dice[2], "die with value 2 is unused");
            moves[0].Lane.Should().BeSameAs(board.Lanes[20], "there is only one piece for this player");
            moves[0].LaneIndex.Should().Be(20, "the player's only piece is on that lane");
            moves[1].Die.Should().BeSameAs(dice[3], "die with value 1 is unused and the target has only 1 enemy piece");
            moves[1].Lane.Should().BeSameAs(board.Lanes[20], "there is only one piece for this player");
            moves[1].LaneIndex.Should().Be(20, "the player's only piece is on that lane");

            var piece = board.Lanes[20][0];
            var enemyPiece = board.Lanes[21][0];
            strategy.MovePiece(player, board, moves[1]);
            board.Bar.Should().ContainSingle("the enemy piece should have been moved to the bar");
            board.Bar[0].Should().BeSameAs(enemyPiece, "the enemy piece should be the same as the removed one");
            board.Lanes[21].Should().ContainSingle("the piece should have been moved to this lane");
            board.Lanes[21][0].Should().BeSameAs(piece, "the moved piece should be the same as the selected one");
        }

        [Fact]
        public void TestBearOffPieceStrategyZeroDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(false, 6), new DefaultDie(false, 5) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Lanes[17].Add(new DefaultPiece(player));
            board.Lanes[18].Add(new DefaultPiece(player));
            board.Lanes[19].Add(new DefaultPiece(enemy));
            var strategy = new BearOffPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Should().BeEmpty("there should be no moves available");
        }

        [Fact]
        public void TestBearOffPieceStrategyOneDie()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(true, 6), new DefaultDie(false, 5), new DefaultDie(false, 4), new DefaultDie(false, 3) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Lanes[19].Add(new DefaultPiece(player));
            board.Lanes[20].Add(new DefaultPiece(enemy));
            var strategy = new BearOffPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Should().ContainSingle("there should be only one move available");
            moves[0].Die.Should().Be(dice[1], "this is the only available die which can be used to exit");
            moves[0].Lane.Should().BeSameAs(board.Lanes[19], "this lane contains the only piece for the player");
            moves[0].LaneIndex.Should().Be(19, "this lane contains the only piece for the player");

            var piece = board.Lanes[19][0];
            strategy.MovePiece(player, board, moves[0]);
            board.Lanes[19].Should().BeEmpty("the piece should have been beared off");
            board.BearedOff.Should().ContainSingle("the piece should have been moved to this lane");
            board.BearedOff[0].Should().BeSameAs(piece, "the beared off piece should be the same as the selected one");
        }

        [Fact]
        public void TestBearOffPieceStrategyTwoDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var enemy = this.app.GetGameFactory().CreatePlayer("Player 2");
            var dice = new[] { new DefaultDie(false, 6), new DefaultDie(false, 5), new DefaultDie(false, 4), new DefaultDie(false, 3) };
            var turn = new DefaultTurn(player, dice);
            var context = new DefaultGameRuleStrategyContext();
            var board = this.CreateDefaultBoard();
            board.Lanes[19].Add(new DefaultPiece(player));
            board.Lanes[19].Add(new DefaultPiece(player));
            board.Lanes[20].Add(new DefaultPiece(enemy));
            var strategy = new BearOffPieceStrategy();

            var moves = this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeFalse("the strategy should never short-circuit");
            moves.Count.Should().Be(2, "there should be two move available");
            moves[0].Die.Should().Be(dice[0], "a larger move can be used when there is no piece before ours");
            moves[0].Lane.Should().BeSameAs(board.Lanes[19], "this lane contains the only piece for the player");
            moves[0].LaneIndex.Should().Be(19, "this lane contains the only piece for the player");
            moves[1].Die.Should().Be(dice[1], "an exact die can be used to bear off the piece");
            moves[1].Lane.Should().BeSameAs(board.Lanes[19], "this lane contains the only piece for the player");
            moves[1].LaneIndex.Should().Be(19, "this lane contains the only piece for the player");

            var unmovedPiece = board.Lanes[19][0];
            var piece = board.Lanes[19][1];
            strategy.MovePiece(player, board, moves[0]);
            board.Lanes[19].Should().ContainSingle("only one piece should be beared off");
            board.Lanes[19][0].Should().BeSameAs(unmovedPiece, "the remaining piece should not be changed");
            board.BearedOff.Should().ContainSingle("the piece should have been moved to this lane");
            board.BearedOff[0].Should().BeSameAs(piece, "the beared off piece should be the same as the selected one");
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

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
