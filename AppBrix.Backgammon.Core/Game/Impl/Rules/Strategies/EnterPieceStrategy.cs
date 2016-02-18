// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules.Strategies
{
    internal class EnterPieceStrategy : GameRuleStrategyBase
    {
        #region Public and overriden methods
        protected override void GetStrategyValidMoves(IGameBoard board, ITurn turn, IGameRuleStrategyContext context)
        {
            if (board.Bar.Pieces.Count > 0 && board.Bar.Pieces[0].Player == turn.Player)
            {
                foreach (var die in this.GetAvailableDice(turn.Dice))
                {
                    if (this.IsMoveValid(board, die.Value, turn.Player))
                    {
                        context.Moves.Add(new DefaultMove(board.Bar, -1, die));
                    }
                }
                context.IsDone = true;
            }
        }

        protected override bool MakeMove(IPlayer player, IGameBoard board, IGameMove move)
        {
            if (move.Lane != board.Bar || !this.IsMoveValid(board, move.Die.Value, player.Name))
                return false;
            
            var targetLane = board.Lanes[move.Die.Value - 1];
            if (targetLane.Pieces.Count == 1 && targetLane.Pieces[0].Player != player.Name)
            {
                targetLane.MovePiece(board.Bar);
            }
            move.Lane.MovePiece(targetLane);
            return true;
        }
        #endregion

        #region Private methods
        private bool IsMoveValid(IBoard board, int die, string player)
        {
            var targetLane = board.Lanes[die - 1];
            return targetLane.Pieces.Count <= 1 || targetLane.Pieces[0].Player == player;
        }
        #endregion
    }
}
