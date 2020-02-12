// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Rules.Strategies
{
    internal abstract class GameRuleStrategyBase
    {
        #region Public and abstract methods
        public GameRuleStrategyBase SetNext(GameRuleStrategyBase rule)
        {
            this.next = rule;
            return rule;
        }

        public IEnumerable<IMove> GetAvailableMoves(IBoard board, ITurn turn)
        {
            if (!turn.AreDiceRolled || !this.GetAvailableDice(turn.Dice).Any())
                yield break;

            var context = new DefaultGameRuleStrategyContext();
            for (var strategy = this; !context.IsDone && strategy != null; strategy = strategy.next)
            {
                foreach (var move in strategy.GetStrategyAvailableMoves(board, turn, context))
                {
                    yield return move;
                }
            }
        }

        protected internal abstract IEnumerable<IMove> GetStrategyAvailableMoves(IBoard board, ITurn turn, IGameRuleStrategyContext context);

        public bool CanMovePiece(IPlayer player, IBoard board, IMove move)
        {
            var context = new DefaultGameRuleStrategyContext();
            var canMovePiece = false;
            for (var strategy = this; !context.IsDone && !canMovePiece && strategy != null; strategy = strategy.next)
            {
                canMovePiece = strategy.CanStrategyMovePiece(player, board, move, context);
            }
            return canMovePiece;
        }

        protected abstract bool CanStrategyMovePiece(IPlayer player, IBoard board, IMove move, IGameRuleStrategyContext context);

        public void MovePiece(IPlayer player, IGameBoard board, IGameMove move)
        {
            for (var strategy = this; strategy != null && !strategy.MakeMove(player, board, move); strategy = strategy.next)
            {
            }
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
