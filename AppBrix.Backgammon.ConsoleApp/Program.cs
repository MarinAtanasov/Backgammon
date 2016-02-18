// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core;
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Game;
using AppBrix.Configuration;
using AppBrix.Configuration.Files;
using AppBrix.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AppBrix.Backgammon.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var app = App.Create(new ConfigManager(new FilesConfigProvider("./Config", "json"), new JsonConfigSerializer()));
            app.Start();
            try
            {
                Program.Run(app);
            }
            catch (Exception ex)
            {
                app.GetLog().Error("The application has stopped because of an error!", ex);
            }
            finally
            {
                app.Stop();
                Console.WriteLine("Executed in: {0} seconds.", stopwatch.Elapsed.TotalSeconds);
            }
        }

        private static void Run(IApp app)
        {
            var players = new Dictionary<string, IPlayer>
            {
                { "Player 1", app.Get<IGameFactory>().CreatePlayer("Player 1") },
                { "Player 2", app.Get<IGameFactory>().CreatePlayer("Player 2") }
            };
            var game = app.Get<IGameFactory>().CreateGame(players.Values.ToList());
            game.Start(players.Values.First());

            while (game.IsRunning)
            {
                Program.OnTurnChanged(game, players);
            }
            Program.OnGameEnded(game);
        }
        
        private static void OnTurnChanged(IGame game, IDictionary<string, IPlayer> players)
        {
            var turn = game.Turn;
            var player = players[turn.Player];

            Program.PrintBoard(game, turn, player);
            if (!turn.AreDiceRolled)
            {
                Console.Write("Press <Enter> to roll rice.");
                Console.ReadLine();
                game.RollDice(player);
            }
            else if (game.AllowedMoves.Count == 0)
            {
                Console.Write("Press <Enter> to end turn.");
                Console.ReadLine();
                game.EndTurn(player);
            }
            else
            {
                var board = game.GetBoard(player);
                bool isValidMove = false;
                do
                {
                    try
                    {
                        var move = Program.SelectMove(game, board);
                        if (move == null)
                            throw new InvalidOperationException("Illegal move!");

                        game.PlayMove(player, move);

                        isValidMove = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                } while (!isValidMove);
            }
        }

        private static void OnGameEnded(IGame game)
        {
            Console.WriteLine("Game ended! Winner: {0}", game.Winner);
        }

        private static void PrintBoard(IGame game, ITurn turn, IPlayer player)
        {
            var board = game.GetBoard(player);
            var lanes = board.Lanes;
            var playerName = player.Name;

            Console.WriteLine("--1--2--3--4--5--6--0---7--8--9-10-11-12-");
            
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < lanes.Count / 4; column++)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("|");
                    Console.Write("{0,1}", pieces.Count > row + 5 ? (pieces[row + 5].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("{0,1}", pieces.Count > row ? (pieces[row].Player == playerName ? "W" : "B") : string.Empty);
                }
                Console.Write("| ");
                Console.Write(board.Bar.Pieces.Count > row ? (board.Bar.Pieces[row].Player == playerName ? "W" : "B") : "-");
                Console.Write(" |");
                for (int column = lanes.Count / 4; column < lanes.Count / 2; column++)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("{0,1}", pieces.Count > row + 5 ? (pieces[row + 5].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("{0,1}", pieces.Count > row ? (pieces[row].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            var dashes = 16 - player.Name.Length;
            Console.Write("-{0} {1} {2}--------", new string('-', dashes / 2), player.Name, new string('-', (dashes + 1) / 2));
            for (int i = 0; i < 4; i++)
            {
                if (turn.Dice.Count > i && !turn.Dice[i].IsUsed)
                {
                    Console.Write("{0}--", turn.Dice[i].Value);
                }
                else
                {
                    Console.Write("---");
                }
            }
            Console.WriteLine("--");

            for (int row = 4; row >= 0; row--)
            {
                for (int column = lanes.Count - 1; column >= (lanes.Count * 3) / 4; column--)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("|");
                    Console.Write("{0,1}", pieces.Count > row + 5 ? (pieces[row + 5].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("{0,1}", pieces.Count > row ? (pieces[row].Player == playerName ? "W" : "B") : string.Empty);
                }
                Console.Write("| - |");
                for (int column = ((lanes.Count * 3) / 4) - 1; column >= lanes.Count / 2; column--)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("{0,1}", pieces.Count > row + 5 ? (pieces[row + 5].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("{0,1}", pieces.Count > row ? (pieces[row].Player == playerName ? "W" : "B") : string.Empty);
                    Console.Write("|");
                }
                Console.WriteLine();
            }

            Console.WriteLine("-24-23-22-21-20-19-----18-17-16-15-14-13-");
        }

        private static IMove SelectMove(IGame game, IBoard board)
        {
            Console.Write("Select \"<position> <die>\" to play: ");
            var toPlay = Console.ReadLine().Split(' ');
            var index = int.Parse(toPlay[0]) - 1;
            var lane = index >= 0 ? board.Lanes[index] : board.Bar;
            var die = int.Parse(toPlay[1]);
            return game.AllowedMoves.FirstOrDefault(x => x.LaneIndex == index && x.Die.Value == die);
        }
    }
}
