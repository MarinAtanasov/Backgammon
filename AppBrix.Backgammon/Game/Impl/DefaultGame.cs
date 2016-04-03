// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Events.Impl;
using AppBrix.Backgammon.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Game.Impl
{
    internal class DefaultGame : IGame
    {
        #region Construction
        public DefaultGame(IApp app, IGameBoard board, IPlayer player1, IPlayer player2, IGameRules gameRules)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (player1 == null)
                throw new ArgumentNullException(nameof(player1));
            if (player2 == null)
                throw new ArgumentNullException(nameof(player2));
            if (player1 == player2)
                throw new ArgumentException(string.Concat(nameof(player1), " == ", nameof(player2)));
            if (player1.Name == player2.Name)
                throw new ArgumentException(string.Format("{0}.{1} == {2}.{3}",
                    nameof(player1), nameof(player1.Name), nameof(player2), nameof(player2.Name)));
            
            this.App = app;
            this.Players = new IPlayer[] { player1, player2 };
            
            var reversedBoard = new DefaultBoard(new ReversedLanes(board.Lanes), board.Bar, board.BearedOff);
            this.Boards = new IGameBoard[] { board, reversedBoard };

            this.Rules = gameRules;
        }
        #endregion

        #region Properties
        public IApp App { get; }

        public IList<IGameBoard> Boards { get; }

        public bool HasEnded { get { return this.Winner != null; } }

        public bool HasStarted { get { return this.Turn != null; } }

        public IList<IPlayer> Players { get; }

        public IGameRules Rules { get; }
        
        public ITurn Turn { get; private set; }

        public string Winner { get; private set; }
        #endregion

        #region Public and overriden methods
        public void Start(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);

            if (this.HasStarted)
                throw new InvalidOperationException("Game already started.");
            
            this.Turn = this.CreateNewTurn(player);
            this.App.GetEventHub().Raise(new DefaultTurnChanged(this));
        }

        public IBoard GetBoard(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            return this.GetBoardInternal(player);
        }

        public IEnumerable<IMove> GetAvailableMoves(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);

            if (!this.HasStarted)
                throw new InvalidOperationException("Game has not started yet.");
            if (this.Turn.Player != player.Name)
                throw new InvalidOperationException("This player's turn has not come yet: " + player);

            return this.Rules.GetAvailableMoves(this.GetBoardInternal(player), this.Turn);
        }

        public void RollDice(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);

            if (!this.HasStarted)
                throw new InvalidOperationException("Game has not started yet.");
            if (this.GetCurrentPlayer() != player)
                throw new InvalidOperationException("This player's turn has not come yet: " + player);
            if (this.Turn.AreDiceRolled)
                throw new InvalidOperationException("Dice have already been rolled this turn.");
            if (this.HasEnded)
                throw new InvalidOperationException("The game is already finished.");

            this.Turn = this.RollDice();
            this.App.GetEventHub().Raise(new DefaultTurnChanged(this));
        }

        public void PlayMove(IPlayer player, IMove move)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (move == null)
                throw new ArgumentNullException(nameof(move));

            if (!this.HasStarted)
                throw new InvalidOperationException("Game has not started yet.");
            if (this.GetCurrentPlayer() != player)
                throw new InvalidOperationException("This player's turn has not come yet: " + player);
            if (!this.Rules.CanMakeMove(player, this.GetBoardInternal(player), move))
                throw new InvalidOperationException("Illegal move!");

            var board = this.GetBoardInternal(player);
            this.Rules.MovePiece(player, board, (IGameMove)move);
            var turn = this.UseDie(player, move.Die);
            
            var winner = this.Rules.TryGetWinner(board, move, this.Players);
            if (winner != null)
                this.Winner = winner.Name;

            this.Turn = turn;
            this.App.GetEventHub().Raise(new DefaultTurnChanged(this));

            if (winner != null)
                this.App.GetEventHub().Raise(new DefaultGameEnded(this));
        }
        
        public void EndTurn(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);

            if (!this.HasStarted)
                throw new InvalidOperationException("Game has not started yet.");
            if (this.GetCurrentPlayer() != player)
                throw new InvalidOperationException("This player's turn has not come yet: " + player);
            if (this.Rules.GetAvailableMoves(this.GetBoardInternal(player), this.Turn).Any())
                throw new InvalidOperationException("The player has not played all his dice.");

            var otherPlayer = this.Players[0] == player ? this.Players[1] : this.Players[0];
            this.Turn = this.CreateNewTurn(otherPlayer);
            this.App.GetEventHub().Raise(new DefaultTurnChanged(this));
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
            return this.App.GetDiceRoller().RollDie();
        }
        
        private ITurn UseDie(IPlayer player, IDie usedDie)
        {
            return new DefaultTurn(player, this.Turn.Dice.Select(x => x == usedDie ? new DefaultDie(true, x.Value) : x).ToList());
        }
        #endregion
    }
}
