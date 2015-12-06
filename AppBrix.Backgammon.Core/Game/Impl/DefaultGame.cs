// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game.Impl.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGame : IGame
    {
        #region Construction
        public DefaultGame(IApp app, IGameBoard board, IPlayer player1, IPlayer player2)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (player1 == null)
                throw new ArgumentNullException("player1");
            if (player2 == null)
                throw new ArgumentNullException("player2");
            if (player1 == player2)
                throw new ArgumentException("player1 == player2");
            if (player1.Name == player2.Name)
                throw new ArgumentException("player1.Name == player2.Name");

            this.app = app;
            this.Players = new IPlayer[] { player1, player2 };
            
            var reversedBoard = new DefaultBoard(new ReversedLanes(board.GameLanes), board.GameBar, board.GameBearedOff);
            this.Boards = new IGameBoard[] { board, reversedBoard };

            this.Turn = this.CreateNewTurn(this.Players.First());
            this.Rules = new BasicGameRules();
        }
        #endregion

        #region Properties
        public IList<IGameBoard> Boards { get; private set; }

        public IList<IPlayer> Players { get; private set; }

        public IGameRules Rules { get; private set; }

        public ITurn Turn
        {
            get
            {
                return this.turn;
            }
            private set
            {
                this.turn = value;
                if (this.TurnChanged != null && !this.IsGameFinished)
                    this.TurnChanged(this.Turn);
            }
        }

        public bool IsGameFinished { get; private set; }
        #endregion

        #region Events
        public event Action<ITurn> TurnChanged;

        public event Action<IGameResult> GameFinished;
        #endregion

        #region Public and overriden methods
        public IBoard GetBoard(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            
            return this.GetBoardInternal(player);
        }

        public void EndTurn(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);

            // TODO: Handle if unable to use dice.
            if (this.turn.Dice.Any(x => !x.IsUsed))
                throw new InvalidOperationException("The player has not played all his dice.");

            var otherPlayer = this.Players[0] == player ? this.Players[1] : this.Players[0];
            this.Turn = this.CreateNewTurn(otherPlayer);
        }

        public void PlayDie(IPlayer player, IBoardLane lane, IDie die)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);
            if (lane == null)
                throw new ArgumentNullException("lane");
            var board = this.GetBoardInternal(player);
            if (board.Bar != lane && !board.Lanes.Contains(lane))
                throw new ArgumentException("Lane not found: " + lane);
            if (die == null)
                throw new ArgumentNullException("die");
            if (!this.Turn.Dice.Contains(die))
                throw new ArgumentException("Die not found: " + die);
            if (die.IsUsed)
                throw new ArgumentException("This die has already been used: " + die);

            if (this.IsGameFinished)
                throw new InvalidOperationException("The game is already finished.");

            this.Rules.MovePiece(player, board, (IGameBoardLane)lane, die);
            var turn = this.UseDie(player, die);
            // TODO: Handle if unable to use dice.

            var winner = this.Rules.TryGetWinner(board, this.Players);
            this.IsGameFinished = winner != null;
            this.Turn = turn;

            if (this.IsGameFinished)
                this.GameFinished(new DefaultGameResult(winner));
        }

        public void RollDice(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);

            if (this.Turn.AreDiceRolled)
                throw new InvalidOperationException("Dice have already been rolled this turn.");
            if (this.IsGameFinished)
                throw new InvalidOperationException("The game is already finished.");

            this.Turn = this.RollDice();
            // TODO: Handle if unable to use dice.
        }
        #endregion

        #region Private methods
        private IPlayer GetCurrentPlayer()
        {
            return this.Players[0].Name == this.Turn.Player ? this.Players[0] : this.Players[1];
        }

        private IGameBoard GetBoardInternal(IPlayer player)
        {
            var index = this.Players.IndexOf(player);
            if (index < 0)
                throw new ArgumentException("Player not found: " + player);

            return this.Boards[index];
        }

        private ITurn CreateNewTurn(IPlayer player)
        {
            return new DefaultTurn(player, new IDie[0]);
        }

        private ITurn RollDice()
        {
            var dice = new List<IDie>(2);
            dice.Add(new DefaultDie(false, this.RollDie()));
            dice.Add(new DefaultDie(false, this.RollDie()));
            if (dice[0].Value == dice[1].Value)
            {
                dice.Add(new DefaultDie(false, dice[0].Value));
                dice.Add(new DefaultDie(false, dice[0].Value));
            }
            return new DefaultTurn(this.GetCurrentPlayer(), dice);
        }

        private int RollDie()
        {
            return app.GetDiceRoller().RollDie();
        }

        private ITurn UseDie(IPlayer player, IDie usedDie)
        {
            return new DefaultTurn(player, this.Turn.Dice.Select(x => x == usedDie ? new DefaultDie(true, x.Value) : x).ToArray());
        }
        #endregion

        #region private fields and constants
        private readonly IApp app;
        private ITurn turn;
        #endregion
    }
}
