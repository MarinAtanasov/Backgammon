// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Events.Impl;
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
            
            var reversedBoard = new DefaultBoard(new ReversedLanes(board.Lanes), board.Bar, board.BearedOff);
            this.Boards = new IGameBoard[] { board, reversedBoard };

            this.Rules = new BasicGameRules();
        }
        #endregion

        #region Properties
        public IList<IGameBoard> Boards { get; private set; }

        public bool IsRunning { get; private set; }

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
                this.SetAllowedMoves();
                this.app.GetEventHub().Raise(new DefaultTurnChanged(this));
            }
        }
        
        public IReadOnlyCollection<IMove> AllowedMoves { get; private set; }

        public string Winner { get; private set; }
        #endregion

        #region Public and overriden methods
        public void Start(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);

            if (this.isStarted)
                throw new InvalidOperationException("Game already started.");

            this.isStarted = true;
            this.IsRunning = true;
            this.Turn = this.CreateNewTurn(player);
        }

        public IBoard GetBoard(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            return this.GetBoardInternal(player);
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
            if (!this.IsRunning)
                throw new InvalidOperationException("The game is already finished.");

            this.Turn = this.RollDice();
        }

        public void PlayMove(IPlayer player, IMove move)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);
            if (move == null)
                throw new ArgumentNullException("move");
            if (!this.AllowedMoves.Contains(move))
                throw new ArgumentException("Illegal move!");

            var board = this.GetBoardInternal(player);
            this.Rules.MovePiece(player, board, (IGameMove)move);
            var turn = this.UseDie(player, move.Die);
            
            var winner = this.Rules.TryGetWinner(board, move, this.Players);
            this.IsRunning = winner == null;
            this.Turn = turn;
            if (!this.IsRunning)
            {
                this.Winner = winner.Name;
                this.app.GetEventHub().Raise(new DefaultGameEnded(this));
            }
        }

        public void EndTurn(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);
            
            if (this.AllowedMoves.Count > 0)
                throw new InvalidOperationException("The player has not played all his dice.");

            var otherPlayer = this.Players[0] == player ? this.Players[1] : this.Players[0];
            this.Turn = this.CreateNewTurn(otherPlayer);
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

        private void SetAllowedMoves()
        {
            if (this.IsRunning)
            {
                this.AllowedMoves = this.Rules.GetValidMoves(this.GetBoardInternal(this.GetCurrentPlayer()), this.Turn);
            }
            else
            {
                this.AllowedMoves = new List<IMove>();
            }
        }

        private ITurn UseDie(IPlayer player, IDie usedDie)
        {
            return new DefaultTurn(player, this.Turn.Dice.Select(x => x == usedDie ? new DefaultDie(true, x.Value) : x).ToArray());
        }
        #endregion

        #region private fields and constants
        private readonly IApp app;
        private ITurn turn;
        private bool isStarted;
        #endregion
    }
}
