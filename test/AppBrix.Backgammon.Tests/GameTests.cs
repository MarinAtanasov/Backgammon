// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Events;
using AppBrix.Backgammon.Game;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AppBrix.Backgammon.Tests
{
    public class GameTests : IDisposable
    {
        #region Setup and cleanup
        public GameTests()
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
        [Fact, Trait(TestCategories.Category, TestCategories.Performance)]
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

        [Fact, Trait(TestCategories.Category, TestCategories.Performance)]
        public void TestPerformanceGameMultithreading()
        {
            // The first game can skew the results because the assemblies are lazily loaded.
            this.PlayGameRandomly();

            var duration = TimeSpan.FromMilliseconds(100);
            var expectedGain = 0.8;
            double gamesOneCore = this.PlayGamesAsync(1, duration);
            double gamesTwoCores = this.PlayGamesAsync(2, duration);
            gamesTwoCores.Should().BeGreaterThan(gamesOneCore, "games should run asynchronously on two cores");
            gamesTwoCores.Should().BeGreaterThan(gamesOneCore * (1 + expectedGain), $"performance gain should be at least {expectedGain * 100}% for the second core");
        }
        #endregion

        #region Private methods
        private int PlayGamesAsync(int threads, TimeSpan time)
        {
            var stopwatch = Stopwatch.StartNew();
            var played = Enumerable.Range(0, threads)
                .Select(x => Task.Factory.StartNew(() =>
                {
                    var games = 0;
                    while (stopwatch.Elapsed < time)
                    {
                        this.PlayGameRandomly();
                        games++;
                    }
                    return games;
                }))
                .ToList()
                .Select(x => x.Result)
                .ToList();
            return played.Sum();
        }

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
                    game.PlayMove(player, moves[this.app.GetRandomService().GetRandom().Next(moves.Count)]);
                }
                else
                {
                    game.EndTurn(player);
                }
            }
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
