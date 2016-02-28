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
        #endregion

        #region Private methods
        private void CallGetStrategyValidMoves(GameRuleStrategyBase strategy, IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            GameRuleTests.GetStrategyValidMovesMethod.Invoke(strategy, new object[] { board, turn, context });
        }
        #endregion

        #region Private fields and constants
        private static readonly MethodInfo GetStrategyValidMovesMethod = typeof(GameRuleStrategyBase).GetTypeInfo().DeclaredMethods.Single(x => x.Name == "GetStrategyValidMoves");
        private readonly IApp app;
        #endregion
    }
}
