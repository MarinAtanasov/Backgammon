// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Impl.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultGame : IGame
    {
        #region Construction
        public DefaultGame(IApp app, IPlayer player1, IPlayer player2)
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
                throw new ArgumentException("player1 == player2");

            this.app = app;
            this.Players = new IPlayer[] { player1, player2 };

            var board = this.CreateBoard();
            this.SetBoard(board, this.Players);
            var reversedBoard = new DefaultBoard(new ReversedLanes(board.GameLanes), board.GameBar, board.GameBearedOff);
            this.Boards = new IGameBoard[] { board, reversedBoard };

            this.Turn = this.CreateNewTurn(this.Players.First());
            this.Rules = new BasicMoveRule();
        }
        #endregion

        #region Properties
        public IList<IGameBoard> Boards { get; private set; }

        public IList<IPlayer> Players { get; private set; }

        public MoveRuleBase Rules { get; private set; }

        public ITurn Turn { get; private set; }
        #endregion

        #region Public and overriden methods
        public IBoard GetBoard(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            var index = this.Players.IndexOf(player);
            if (index < 0)
                throw new ArgumentException("Player not found: " + player);

            return this.Boards[index];
        }

        public ITurn PlayDie(IPlayer player, IBoardLane lane, IDie die)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);
            if (lane == null)
                throw new ArgumentNullException("lane");
            var board = this.GetBoard(player);
            if (board.Bar != lane && !board.Lanes.Contains(lane))
                throw new ArgumentException("Lane not found: " + lane);
            if (die == null)
                throw new ArgumentNullException("die");
            if (!this.Turn.Dice.Contains(die))
                throw new ArgumentException("Die not found: " + die);
            if (die.IsUsed)
                throw new ArgumentException("This die has already been used: " + die);

            var context = this.Rules.CalculatePossibleMoves(board, this.Turn, player);
            
            if (context.CanMove(lane, die))
            {
                this.Turn = this.UseDie(die);
            }
            else
            {
                throw new ArgumentException("Invalid move.");
            }
            return this.Turn;
        }

        public ITurn RollDice(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (!this.Players.Contains(player))
                throw new ArgumentException("Player not found: " + player);
            if (this.GetCurrentPlayer() != player)
                throw new ArgumentException("This player's turn has not come yet: " + player);

            if (this.Turn.AreDiceRolled)
                throw new InvalidOperationException("Dice have already been rolled this turn.");

            this.Turn = this.RollDice();
            return this.Turn;
        }
        #endregion

        #region Board initialization
        private IGameBoard CreateBoard()
        {
            var lanes = new List<IGameBoardLane>(24);

            for (int i = 0; i < 24; i++)
            {
                lanes.Add(new DefaultBoardLane(new IPiece[0]));
            }

            return new DefaultBoard(lanes, new DefaultBoardLane(), new DefaultBoardLane());
        }

        private void SetBoard(IGameBoard board, IList<IPlayer> players)
        {
            var lanes = (IList<IGameBoardLane>)board.GameLanes;
            lanes[0] = new DefaultBoardLane(this.CreatePieces(2, players[0]));
            lanes[5] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[7] = new DefaultBoardLane(this.CreatePieces(3, players[1]));
            lanes[11] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[12] = new DefaultBoardLane(this.CreatePieces(5, players[1]));
            lanes[16] = new DefaultBoardLane(this.CreatePieces(3, players[0]));
            lanes[18] = new DefaultBoardLane(this.CreatePieces(5, players[0]));
            lanes[23] = new DefaultBoardLane(this.CreatePieces(2, players[1]));
        }

        private IPiece[] CreatePieces(int count, IPlayer owner = null)
        {
            var pieces = new IPiece[count];
            for (int i = 0; i < count; i++)
            {
                pieces[i] = new DefaultPiece(owner);
            }
            return pieces;
        }
        #endregion

        #region Private methods
        private IPlayer GetCurrentPlayer()
        {
            return this.Players[0].Name == this.Turn.Player ? this.Players[0] : this.Players[1];
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

        private ITurn UseDie(IDie usedDie)
        {
            IPlayer currentPlayer = this.Players[0];
            IPlayer otherPlayer = this.Players[1];

            if (this.Players[0].Name != this.Turn.Player)
            {
                currentPlayer = this.Players[1];
                otherPlayer = this.Players[0];
            }
            
            if (this.Turn.Dice.Count(x => x.IsUsed) < this.Turn.Dice.Count - 1)
                return new DefaultTurn(currentPlayer, this.Turn.Dice.Select(x => x == usedDie ? new DefaultDie(true, x.Value) : x).ToArray());
            else if (this.Turn.Dice.Count == 4)
                return this.CreateNewTurn(currentPlayer);
            else
                return this.CreateNewTurn(otherPlayer);
        }
        #endregion

        #region private fields and constants
        private readonly IApp app;
        #endregion
    }
}
