// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules
{
    internal sealed class BasicGameRules : IGameRules
    {
        #region Public and overriden methods
        public bool IsMoveValid(IPlayer player, IBoard board, IBoardLane lane, IDie die)
        {
            var playerName = player.Name;
            if (lane.Pieces.Count == 0 || lane.TopPiece.Player != playerName)
            {
                return false;
            }

            var lanes = board.Lanes;
            var dieValue = die.Value;
            if (lane == board.Bar)
            {
                return this.CanEnterPiece(lanes, playerName, dieValue);
            }

            if (this.NeedToEnterPiece(board, playerName))
            {
                return false;
            }

            int index = this.GetLaneIndex(lanes, lane);
            if (index + dieValue < lanes.Count)
            {
                return this.CanMovePiece(playerName, lanes[index + dieValue]);
            }
            else
            {
                return !this.NeedToMovePiece(lanes, playerName) && this.CanBearOffPiece(lanes, playerName, index, dieValue);
            }
        }

        public void MovePiece(IPlayer player, IGameBoard board, IGameBoardLane lane, IDie die)
        {
            if (!this.IsMoveValid(player, board, lane, die))
            {
                throw new InvalidOperationException("Illegal move!");
            }

            IPiece piece;
            IGameBoardLane targetLane;

            var playerName = player.Name;
            var lanes = board.GameLanes;
            var dieValue = die.Value;
            int index = this.GetLaneIndex(lanes, lane);

            if (lane == board.Bar)
            {
                piece = lane.Pieces.Last(x => x.Player == playerName);
                targetLane = lanes[dieValue - 1];
            }
            else
            {
                piece = lane.TopPiece;
                var newIndex = index + dieValue;
                targetLane = newIndex < lanes.Count ? lanes[newIndex] : board.GameBearedOff;
            }

            lane.RemovePiece(piece);

            if (targetLane != board.GameBearedOff && targetLane.Pieces.Count == 1 && targetLane.TopPiece.Player != playerName)
            {
                var barredPiece = targetLane.TopPiece;
                targetLane.RemovePiece(barredPiece);
                board.GameBar.AddPiece(barredPiece);
            }

            targetLane.AddPiece(piece);
        }
        #endregion

        #region Private methods
        private bool CanEnterPiece(IReadOnlyList<IBoardLane> lanes, string playerName, int dieValue)
        {
            var targetLane = lanes[dieValue - 1];
            return targetLane.Pieces.Count <= 1 || targetLane.TopPiece.Player == playerName;
        }

        private bool CanMovePiece(string playerName, IBoardLane targetLane)
        {
            return targetLane.Pieces.Count <= 1 || targetLane.TopPiece.Player == playerName;
        }

        private bool CanBearOffPiece(IReadOnlyList<IBoardLane> lanes, string playerName, int laneIndex, int dieValue)
        {
            if (laneIndex + dieValue == lanes.Count)
            {
                return true;
            }

            for (int i = (lanes.Count * 3) / 4; i < laneIndex; i++)
            {
                if (lanes[i].Pieces.Count > 0 && lanes[i].TopPiece.Player == playerName)
                {
                    return false;
                }
            }

            return true;
        }

        private int GetLaneIndex(IReadOnlyList<IBoardLane> lanes, IBoardLane lane)
        {
            for (int i = 0; i < lanes.Count; i++)
            {
                if (lane == lanes[i])
                {
                    return i;
                }
            }

            return -1;
        }

        private bool NeedToEnterPiece(IBoard board, string playerName)
        {
            return board.Bar.Pieces.Any(x => x.Player == playerName);
        }

        private bool NeedToMovePiece(IReadOnlyList<IBoardLane> lanes, string playerName)
        {
            for (int i = 0; i < (lanes.Count * 3) / 4; i++)
            {
                if (lanes[i].Pieces.Count > 0 && lanes[i].TopPiece.Player == playerName)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
