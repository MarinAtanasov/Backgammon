// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl.Rules
{
    internal abstract class MoveRuleBase
    {
        public MoveRuleContext CalculatePossibleMoves(IBoard board, ITurn turn, IPlayer player)
        {
            var context = new MoveRuleContext();
            var moveRule = this.GetFirst();
            while (moveRule != null && !context.HasMoves)
            {
                moveRule.CalculatePossibleMovesInternal(board, turn, player, context);
                moveRule = moveRule.next;
            }
            return context;
        }

        protected abstract void CalculatePossibleMovesInternal(IBoard board, ITurn turn, IPlayer player, MoveRuleContext context);

        public MoveRuleBase GetFirst()
        {
            var moveRule = this;
            while (moveRule.previous != null)
            {
                moveRule = moveRule.previous;
            }
            return moveRule;
        }

        public MoveRuleBase SetNext(MoveRuleBase next)
        {
            this.next = next;
            this.next.previous = this;
            return this;
        }

        private MoveRuleBase next;
        private MoveRuleBase previous;
    }
}
