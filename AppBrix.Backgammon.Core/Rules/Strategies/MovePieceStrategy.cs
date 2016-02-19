// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using AppBrix.Backgammon.Core.Game.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules.Strategies
{
    internal class MovePieceStrategy : GameRuleStrategyBase
    {
        #region Public and overriden methods
        protected override void GetStrategyValidMoves(IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            var playerName = turn.Player;
            var dice = this.GetAvailableDice(turn.Dice).ToList();
            for (int i = 0; i < board.Lanes.Count; i++)
            {
                var lane = board.Lanes[i];
                if (lane.Pieces.Count == 0 || lane.Pieces[0].Player != playerName)
                    continue;

                foreach (var die in dice)
                {
                    if (this.IsMoveValid(board, i, die.Value, playerName))
                    {
                        context.Moves.Add(new DefaultMove(lane, i, die));
                    }
                }
            }
        }

        protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
        {
            if (!this.IsMoveValid(board, move.LaneIndex, move.Die.Value, player.Name))
                return false;

            var targetLane = board.Lanes[move.LaneIndex + move.Die.Value];
            if (targetLane.Pieces.Count == 1 && targetLane.Pieces[0].Player != player.Name)
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
                if (targetLane.Pieces.Count <= 1 || targetLane.Pieces[0].Player == player)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
