// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Events;
using AppBrix.Backgammon.Game;
using AppBrix.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AppBrix.Backgammon.Tests;

[TestClass]
public class GameTests : TestsBase<BackgammonModule>
{
    #region Test lifecycle
    protected override void Initialize() => this.App.Start();
    #endregion

    #region Tests
    [Test, Performance]
    public void TestPerformanceGame()
    {
        // The first game can skew the results because the assemblies are lazily loaded.
        this.PlayGameRandomly();

        var stopwatch = Stopwatch.StartNew();
        var times = new List<double>();
        this.App.GetEventHub().Subscribe<IGameEnded>(x =>
        {
            times.Add(stopwatch.Elapsed.TotalMilliseconds);
            stopwatch.Restart();
        });

        for (int i = 0; i < 100; i++)
        {
            this.PlayGameRandomly();
        }

        stopwatch.Reset();
        this.Assert(times.Average() < 3, "this tests the average performance per game");
    }

    [Test, Performance]
    public void TestPerformanceGameMultithreading()
    {
        // The first game can skew the results because the assemblies are lazily loaded.
        this.PlayGameRandomly();

        var duration = TimeSpan.FromMilliseconds(100);
        var expectedGain = 0.25;
        double gamesOneCore = this.PlayGamesAsync(1, duration);
        double gamesTwoCores = this.PlayGamesAsync(2, duration);
        this.Assert(gamesTwoCores > gamesOneCore, "games should run asynchronously on two cores");
        this.Assert(gamesTwoCores > gamesOneCore * (1 + expectedGain), $"performance gain should be at least {expectedGain * 100}% for the second core");
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
            .Select(x => x.GetAwaiter().GetResult())
            .ToList();
        return played.Sum();
    }

    private void PlayGameRandomly()
    {
        var player1Name = "Player 1";
        var player2Name = "Player 2";
        var players = new Dictionary<string, IPlayer>
            {
                { player1Name, this.App.Get<IGameFactory>().CreatePlayer(player1Name) },
                { player2Name, this.App.Get<IGameFactory>().CreatePlayer(player2Name) }
            };
        var game = this.App.Get<IGameFactory>().CreateGame(players.Values.ToList());
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
                game.PlayMove(player, moves[this.App.GetRandomService().GetRandom().Next(moves.Count)]);
            }
            else
            {
                game.EndTurn(player);
            }
        }
    }
    #endregion
}
