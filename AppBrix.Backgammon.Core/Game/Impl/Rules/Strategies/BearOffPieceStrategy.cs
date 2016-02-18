// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules.Strategies
{
    internal class BearOffPieceStrategy : GameRuleStrategyBase
    {
        #region Public and overriden methods
        protected override void GetStrategyValidMoves(IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            if (this.NeedToMovePiece(board, turn.Player))
                return;

            var playerName = turn.Player;
            var dice = this.GetAvailableDice(turn.Dice).OrderByDescending(x => x.Value).ToList();
            var hasExactMove = false;
            for (int i = (board.Lanes.Count * 3) / 4; i < board.Lanes.Count; i++)
            {
                var lane = board.Lanes[i];
                if (lane.Pieces.Count == 0 || lane.Pieces[0].Player != playerName)
                    continue;

                foreach (var die in dice)
                {
                    hasExactMove = hasExactMove || i + die.Value == board.Lanes.Count;
                    if (i + die.Value == board.Lanes.Count ||
                        (i + die.Value > board.Lanes.Count && !hasExactMove))
                    {
                        context.Moves.Add(new DefaultMove(board.Lanes[i], i, die));
                    }
                }
            }
        }

        protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
        {
            move.Lane.MovePiece(board.BearedOff);
            return true;
        }
        #endregion

        #region Private methods
        private bool NeedToMovePiece(IBoard board, string playerName)
        {
            var lanes = board.Lanes;
            var preBearOffLanes = (lanes.Count * 3) / 4;
            for (int i = 0; i < preBearOffLanes; i++)
            {
                if (lanes[i].Pieces.Count > 0 && lanes[i].Pieces[0].Player == playerName)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
