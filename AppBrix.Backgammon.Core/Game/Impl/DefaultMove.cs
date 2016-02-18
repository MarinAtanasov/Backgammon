// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Linq;
using AppBrix.Backgammon.Core.Board.Impl;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultMove : IGameMove
    {
        #region Construction
        public DefaultMove(IGameBoardLane lane, int laneIndex, IDie die)
        {
            if (lane == null)
                throw new ArgumentNullException("lane");
            if (die == null)
                throw new ArgumentNullException("die");

            this.Lane = lane;
            this.LaneIndex = laneIndex;
            this.Die = die;
        }
        #endregion

        #region Properties
        public IGameBoardLane Lane { get; private set; }

        IBoardLane IMove.Lane { get { return this.Lane; } }

        public int LaneIndex { get; private set; }

        public IDie Die { get; private set; }
        #endregion
    }
}
