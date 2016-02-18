// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game.Impl.Rules.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules
{
    internal sealed class BasicGameRules : IGameRules
    {
        #region Construction
        public BasicGameRules()
        {
            this.strategy = new EnterPieceStrategy();
            this.strategy.SetNext(new MovePieceStrategy())
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

        public IPlayer TryGetWinner(IBoard board, IEnumerable<IPlayer> players)
        {
            var playersOnBoard = board.Lanes.SelectMany(l => l.Pieces).Select(x => x.Player).Distinct().ToList();
            return players.FirstOrDefault(x => !playersOnBoard.Contains(x.Name));
        }
        #endregion
        
        #region Private fields and constants
        private GameRuleStrategyBase strategy;
        #endregion
    }
}
