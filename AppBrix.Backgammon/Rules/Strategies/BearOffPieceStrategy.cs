// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using AppBrix.Backgammon.Game.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Rules.Strategies
{
    internal class BearOffPieceStrategy : GameRuleStrategyBase
    {
        #region Public and overriden methods
        protected internal override IEnumerable<IMove> GetStrategyAvailableMoves(IBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            if (this.NeedToMovePiece(board, turn.Player))
                yield break;

            var playerName = turn.Player;
            var dice = this.GetAvailableDice(turn.Dice).OrderByDescending(x => x.Value).ToList();
            var firstPiece = false;
            for (int i = (board.Lanes.Count * 3) / 4; i < board.Lanes.Count; i++)
            {
                var lane = board.Lanes[i];
                if (lane.Count == 0 || lane[0].Player != playerName)
                    continue;

                foreach (var die in dice)
                {
                    if (i + die.Value == board.Lanes.Count ||
                        (i + die.Value > board.Lanes.Count && !firstPiece))
                    {
                        yield return new DefaultMove(board.Lanes[i], i, die);
                        firstPiece = true;
                    }
                }

                firstPiece = true;
            }
        }

        protected override bool CanStrategyMovePiece(IPlayer player, IBoard board, IMove move, IGameRuleStrategyContext context)
        {
            var targetIndex = move.LaneIndex + move.Die.Value;
            if (targetIndex < board.Lanes.Count || this.NeedToMovePiece(board, player.Name))
                return false;
            else if (targetIndex == board.Lanes.Count)
                return true;
            else
            {
                for (int i = (board.Lanes.Count * 3) / 4; i < move.LaneIndex; i++)
                {
                    var lane = board.Lanes[i];
                    if (lane.Count > 0 && lane[0].Player == player.Name)
                        return false;
                }
                return true;
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
                if (lanes[i].Count > 0 && lanes[i][0].Player == playerName)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
