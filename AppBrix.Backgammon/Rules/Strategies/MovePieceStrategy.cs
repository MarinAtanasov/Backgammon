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
    internal class MovePieceStrategy : GameRuleStrategyBase
    {
        #region Public and overriden methods
        protected internal override IEnumerable<IMove> GetStrategyAvailableMoves(IBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            var playerName = turn.Player;
            var dice = this.GetAvailableDice(turn.Dice).ToList();
            for (int i = 0; i < board.Lanes.Count; i++)
            {
                var lane = board.Lanes[i];
                if (lane.Count == 0 || lane[0].Player != playerName)
                    continue;

                foreach (var die in dice)
                {
                    if (this.IsMoveValid(board, i, die.Value, playerName))
                    {
                        yield return new DefaultMove(lane, i, die);
                    }
                }
            }
        }

        protected override bool CanStrategyMovePiece(IPlayer player, IBoard board, IMove move, IGameRuleStrategyContext context)
        {
            return this.IsMoveValid(board, move.LaneIndex, move.Die.Value, player.Name);
        }

        protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
        {
            if (!this.IsMoveValid(board, move.LaneIndex, move.Die.Value, player.Name))
                return false;

            var targetLane = board.Lanes[move.LaneIndex + move.Die.Value];
            if (targetLane.Count == 1 && targetLane[0].Player != player.Name)
            {
                targetLane.MovePiece(board.Bar);
            }
            move.Lane.MovePiece(targetLane);
            return true;
        }
        #endregion

        #region Private methods
        private bool IsMoveValid(IBoard board, int index, int die, string player)
        {
            if (index + die < board.Lanes.Count)
            {
                var targetLane = board.Lanes[index + die];
                if (targetLane.Count <= 1 || targetLane[0].Player == player)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
