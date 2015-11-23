// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core;
using AppBrix.Configuration;
using AppBrix.Configuration.Files;
using AppBrix.Configuration.Json;
using System;
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
                var game = app.Get<IGameFactory>().CreateGame(new string[] { "Player1", "Player2" });

                Program.PrintBoard(game);
                System.Threading.Thread.Sleep(2000);
                while (true)
                {
                    while (game.Turn.Dice.Any(x => !x.IsUsed))
                    {
                        game.PlayDie(game.Turn.Player, game.Turn.Player.Board.Lanes.First(), game.Turn.Dice.First(x => !x.IsUsed));
                        Program.PrintBoard(game);
                        System.Threading.Thread.Sleep(2000);
                    }
                    game.RollDice(game.Turn.Player);
                    Program.PrintBoard(game);
                    System.Threading.Thread.Sleep(2000);
                }
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

        private static void PrintBoard(IGame game)
        {
            var player1 = game.Players.First();
            var board = player1.Board;
            var lanes = board.Lanes;

            Console.WriteLine("-----------------------------");
            
            for (int row = 0; row < 5; row++)
            {
                for (int column = 0; column < lanes.Count / 4; column++)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("|{0,1}", pieces.Count > row ? (pieces[row].Owner == player1 ? "W" : "B") : string.Empty);
                }
                Console.Write("| - |");
                for (int column = lanes.Count / 4; column < lanes.Count / 2; column++)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("{0,1}|", pieces.Count > row ? (pieces[row].Owner == player1 ? "W" : "B") : string.Empty);
                }
                Console.WriteLine();
            }

            Console.Write("-------------------");
            for (int i = 0; i < 4; i++)
            {
                if (game.Turn.Dice.Count > i && !game.Turn.Dice[i].IsUsed)
                {
                    Console.Write("{0}-", game.Turn.Dice[i].Value);
                }
                else
                {
                    Console.Write("--");
                }
            }
            Console.WriteLine("--");

            for (int row = 4; row >= 0; row--)
            {
                for (int column = lanes.Count - 1; column >= (lanes.Count * 3) / 4; column--)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("|{0,1}", pieces.Count > row ? (pieces[row].Owner == player1 ? "W" : "B") : string.Empty);
                }
                Console.Write("| - |");
                for (int column = ((lanes.Count * 3) / 4) - 1; column >= lanes.Count / 2; column--)
                {
                    var pieces = lanes[column].Pieces;
                    Console.Write("{0,1}|", pieces.Count > row ? (pieces[row].Owner == player1 ? "W" : "B") : string.Empty);
                }
                Console.WriteLine();
            }

            Console.WriteLine("-----------------------------");
        }
    }
}
