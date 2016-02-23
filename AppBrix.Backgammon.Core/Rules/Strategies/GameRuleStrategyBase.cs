﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules.Strategies
{
    internal abstract class GameRuleStrategyBase
    {
        #region Public and abstract methods
        public GameRuleStrategyBase SetNext(GameRuleStrategyBase rule)
        {
            this.next = rule;
            return rule;
        }

        public IReadOnlyCollection<IGameMove> GetValidMoves(IGameBoard board, ITurn turn)
        {
            var context = new DefaultGameRuleStrategyContext();
            for (var strategy = this; !context.IsDone && strategy != null; strategy = strategy.next)
            {
                strategy.GetStrategyValidMoves(board, turn, context);
            }
            return context.Moves;
        }

        protected abstract void GetStrategyValidMoves(IGameBoard board, ITurn turn, IGameRuleStrategyContext context);

        public void MovePiece(IPlayer player, IGameBoard board, IGameMove move)
        {
            for (var strategy = this; strategy != null && !strategy.MakeMove(player, board, move); strategy = strategy.next) ;
        }

        protected abstract bool MakeMove(IPlayer player, IGameBoard board, IGameMove move);
        #endregion

        #region Private methods
        protected IEnumerable<IDie> GetAvailableDice(IEnumerable<IDie> dice)
        {
            return dice.Where(x => !x.IsUsed).Distinct();
        }
        #endregion

        #region Private fields and constants
        private GameRuleStrategyBase next;
        #endregion
    }
}
