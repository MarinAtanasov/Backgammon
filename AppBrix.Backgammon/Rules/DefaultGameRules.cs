// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using AppBrix.Backgammon.Rules.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Rules
{
    internal sealed class DefaultGameRules : IGameRules
    {
        #region Construction
        public DefaultGameRules()
        {
            this.strategy = new EnterPieceStrategy();
            this.strategy
                .SetNext(new MovePieceStrategy())
                .SetNext(new BearOffPieceStrategy());
        }
        #endregion

        #region Public and overriden methods
        public IEnumerable<IMove> GetAvailableMoves(IBoard board, ITurn turn)
        {
            return this.strategy.GetAvailableMoves(board, turn);
        }

        public bool CanMakeMove(IPlayer player, IBoard board, IMove move)
        {
            return this.strategy.CanMovePiece(player, board, move);
        }

        public void MovePiece(IPlayer player, IGameBoard board, IGameMove move)
        {
            this.strategy.MovePiece(player, board, move);
        }

        public IPlayer TryGetWinner(IBoard board, IMove move, IEnumerable<IPlayer> players)
        {
            return move.LaneIndex + move.Die.Value >= board.Lanes.Count ?
                players.FirstOrDefault(x => board.BearedOff.Count(p => p.Player == x.Name) == DefaultGameRules.PiecesPerPlayer) :
                null;
        }
        #endregion

        #region Private fields and constants
        private const int PiecesPerPlayer = 15;
        private GameRuleStrategyBase strategy;
        #endregion
    }
}
