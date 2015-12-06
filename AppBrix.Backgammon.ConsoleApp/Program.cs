// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core;
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
            var players = new Dictionary<string, IPlayer>()
                {
                    {  "Player 1", app.Get<IGameFactory>().CreatePlayer("Player 1") },
                    {  "Player 2", app.Get<IGameFactory>().CreatePlayer("Player 2") },
                };
            var game = app.Get<IGameFactory>().CreateGame(players.Values.ToList());

            Action<ITurn> onTurnChanged = x =>
            {
                var player = players[x.Player];
                Program.PrintBoard(game, player);
                if (!x.AreDiceRolled)
                {
                    Console.Write("Press <Enter> to roll rice.");
                    Console.ReadLine();
                    game.RollDice(player);
                }
                else
                {
                    var board = game.GetBoard(player);
                    bool isValidMove = false;
                    do
                    {
                        try
                        {
                            Console.Write("Select \"<position> <dice>\" to play: ");
                            var toPlay = Console.ReadLine().Split(' ');
                            var index = int.Parse(toPlay[0]);
                            var lane = index > 0 ? board.Lanes[index - 1] : board.Bar;
                            var die = int.Parse(toPlay[1]);
                            game.PlayDie(player, lane, game.Turn.Dice.FirstOrDefault(d => !d.IsUsed && d.Value == die));

                            isValidMove = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } while (!isValidMove);
                }
            };

            game.GameFinished += x => { Console.WriteLine("Game ended! Winner: {0}", x.Winner); };
            game.TurnChanged += onTurnChanged;

            onTurnChanged(game.Turn);
        }

        private static void PrintBoard(IGame game, IPlayer player)
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
                if (game.Turn.Dice.Count > i && !game.Turn.Dice[i].IsUsed)
                {
                    Console.Write("{0}--", game.Turn.Dice[i].Value);
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
    }
}
