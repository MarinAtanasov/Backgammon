// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using AppBrix.Backgammon.Core.Rules.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules
{
    internal sealed class BasicGameRules : IGameRules
    {
        #region Construction
        public BasicGameRules()
        {
            this.strategy = new OptimizingStrategy();
            this.strategy.SetNext(new EnterPieceStrategy())
                .SetNext(new MovePieceStrategy())
                .SetNext(new BearOffPieceStrategy());
        }
        #endregion

        #region Public and overriden methods
        public IReadOnlyCollection<IMove> GetValidMoves(IGameBoard board, ITurn turn)
        {
            return this.strategy.GetValidMoves(board, turn);
        }
        
        public void MovePiece(IPlayer player, IGameBoard board, IGameMove move)
        {
            this.strategy.MovePiece(player, board, move);
        }

        public IPlayer TryGetWinner(IBoard board, IMove move, IEnumerable<IPlayer> players)
        {
            return move.LaneIndex + move.Die.Value >= board.Lanes.Count ?
                players.FirstOrDefault(x => board.BearedOff.Pieces.Count(p => p.Player == x.Name) == BasicGameRules.PiecesPerPlayer) :
                null;
        }
        #endregion

        #region Private fields and constants
        private const int PiecesPerPlayer = 15;
        private GameRuleStrategyBase strategy;
        #endregion
    }
}
