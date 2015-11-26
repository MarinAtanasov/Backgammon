// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl.Rules
{
    internal sealed class BasicMoveRule : MoveRuleBase
    {
        protected override void CalculatePossibleMovesInternal(IBoard board, ITurn turn, IPlayer player, MoveRuleContext context)
        {
            var playerName = player.Name;
            var lanes = board.Lanes;
            foreach (var die in turn.Dice.Where(x => !x.IsUsed))
            {
                for (int i = 0; i < lanes.Count - die.Value; i++)
                {
                    var lane = board.Lanes[i];
                    if ((lane.TopPiece != null && lane.TopPiece.Player == playerName) &&
                        (lanes[i + die.Value].TopPiece == null || lanes[i + die.Value].TopPiece.Player == playerName))
                    {
                        context.AddMove(lane, die);
                    }
                }
            }
        }
    }
}
