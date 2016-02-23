// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules.Strategies
{
    /// <summary>
    /// Strategy used for optimizing the performance of the strategy calculations.
    /// </summary>
    internal class OptimizingStrategy : GameRuleStrategyBase
    {
        protected override void GetStrategyValidMoves(IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            context.IsDone = !turn.AreDiceRolled || !this.GetAvailableDice(turn.Dice).Any();
        }

        protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
        {
            return false;
        }
    }
}
