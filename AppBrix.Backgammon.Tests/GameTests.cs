// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Events;
using AppBrix.Backgammon.Game;
using AppBrix.Events;
using AppBrix.Factory;
using AppBrix.Resolver;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace AppBrix.Backgammon.Tests
{
    public class GameTests
    {
        #region Setup and cleanup
        public GameTests()
        {
            this.app = TestUtils.CreateTestApp(
                typeof(ResolverModule),
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
        public void TestPerformanceGame()
        {
            // The first game can skew the results because the assemblies are lazily loaded.
            this.PlayGameRandomly();

            var stopwatch = Stopwatch.StartNew();
            var times = new List<double>();
            app.GetEventHub().Subscribe<IGameEnded>(x =>
            {
                times.Add(stopwatch.Elapsed.TotalMilliseconds);
                stopwatch.Restart();
            });

            for (int i = 0; i < 100; i++)
            {
                this.PlayGameRandomly();
            }

            stopwatch.Reset();
            times.Average().Should().BeLessThan(3, "this tests the average performange per game");
        }
        #endregion

        #region Private methods
        private void PlayGameRandomly()
        {
            var player1Name = "Player 1";
            var player2Name = "Player 2";
            var players = new Dictionary<string, IPlayer>
            {
                { player1Name, app.Get<IGameFactory>().CreatePlayer(player1Name) },
                { player2Name, app.Get<IGameFactory>().CreatePlayer(player2Name) }
            };
            var game = app.Get<IGameFactory>().CreateGame(players.Values.ToList());
            var boards = new Dictionary<string, IBoard>
            {
                { player1Name, game.GetBoard(players[player1Name]) },
                { player2Name, game.GetBoard(players[player2Name]) },
            };
            game.Start(players.Values.First());

            while (!game.HasEnded)
            {
                var player = players[game.Turn.Player];
                if (!game.Turn.AreDiceRolled)
                {
                    game.RollDice(player);
                    continue;
                }
                
                var moves = game.GetAvailableMoves(player).ToList();
                if (moves.Count > 0)
                {
                    game.PlayMove(player, moves[GameTests.Random.Next(moves.Count)]);
                }
                else
                {
                    game.EndTurn(player);
                }
            }
        }
        #endregion

        #region Private fields and constants
        private static readonly Random Random = new Random(31415);
        private readonly IApp app;
        #endregion
    }
}
