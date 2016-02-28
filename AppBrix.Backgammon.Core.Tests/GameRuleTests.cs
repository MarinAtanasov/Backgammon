// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using AppBrix.Backgammon.Core.Game.Impl;
using AppBrix.Backgammon.Core.Rules.Strategies;
using AppBrix.Events;
using AppBrix.Factory;
using AppBrix.Resolver;
using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace AppBrix.Backgammon.Core.Tests
{
    public class GameRuleTests
    {
        #region Setup and cleanup
        public GameRuleTests()
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
        public void TestOptimizingStrategyNoRolledDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var turn = new DefaultTurn(player, new IDie[0]);
            var context = new DefaultGameRuleStrategyContext();
            this.CallGetStrategyValidMoves(new OptimizingStrategy(), null, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are no rolled dice");
            context.Moves.Should().BeEmpty("there should be no available moves before rolling the dice");
        }

        [Fact]
        public void TestOptimizingStrategyAllUsedDice()
        {
            var player = this.app.GetGameFactory().CreatePlayer("Player 1");
            var turn = new DefaultTurn(player, new[] { new DefaultDie(true, 6), new DefaultDie(true, 5) });
            var context = new DefaultGameRuleStrategyContext();
            this.CallGetStrategyValidMoves(new OptimizingStrategy(), null, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are no unused dice");
            context.Moves.Should().BeEmpty("there should be no available moves after all dice are used");
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

            this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are pieces to enter");
            context.Moves.Should().ContainSingle("there should be one available move because only one dice is unused");
            context.Moves[0].Die.Should().BeSameAs(dice[0], "die with value 6 is unused");
            context.Moves[0].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            context.Moves[0].LaneIndex.Should().Be(-1, "the bar's index should be -1");

            var piece = board.Bar[0];
            strategy.MovePiece(player, board, context.Moves[0]);
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

            this.CallGetStrategyValidMoves(strategy, board, turn, context);
            context.IsDone.Should().BeTrue("the strategy should short-circuit the chain if there are pieces to enter");
            context.Moves.Count.Should().Be(2, "there should be two available move because only one dice is unused");
            context.Moves[0].Die.Should().BeSameAs(dice[0], "die with value 6 is unused");
            context.Moves[0].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            context.Moves[0].LaneIndex.Should().Be(-1, "the bar's index should be -1");
            context.Moves[1].Die.Should().BeSameAs(dice[1], "die with value 5 is unused and the target has only 1 enemy piece");
            context.Moves[1].Lane.Should().BeSameAs(board.Bar, "the piece on the bar needs to be entered");
            context.Moves[1].LaneIndex.Should().Be(-1, "the bar's index should be -1");

            var piece = board.Bar[0];
            var enemyPiece = board.Lanes[4][0];
            strategy.MovePiece(player, board, context.Moves[1]);
            board.Bar.Should().ContainSingle("the enemy piece should have been moved to the bar");
            board.Bar[0].Should().BeSameAs(enemyPiece, "the enemy piece should be the same as the moved one");
            board.Lanes[4].Should().ContainSingle("the piece should have been moved to lane 5");
            board.Lanes[4][0].Should().BeSameAs(piece, "the moved piece should be the same as the removed one");
        }
        #endregion

        #region Private methods
        private void CallGetStrategyValidMoves(GameRuleStrategyBase strategy, IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            GameRuleTests.GetStrategyValidMovesMethod.Invoke(strategy, new object[] { board, turn, context });
        }

        private IGameBoard CreateDefaultBoard()
        {
            return new DefaultBoard();
        }
        #endregion

        #region Private fields and constants
        private static readonly MethodInfo GetStrategyValidMovesMethod = typeof(GameRuleStrategyBase).GetTypeInfo().DeclaredMethods.Single(x => x.Name == "GetStrategyValidMoves");
        private readonly IApp app;
        #endregion
    }
}
