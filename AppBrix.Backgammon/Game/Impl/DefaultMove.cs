// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Game.Impl
{
    internal class DefaultMove : IGameMove
    {
        #region Construction
        public DefaultMove(IBoardLane lane, int laneIndex, IDie die)
        {
            if (lane == null)
                throw new ArgumentNullException(nameof(lane));
            if (die == null)
                throw new ArgumentNullException(nameof(die));

            this.Lane = (IGameBoardLane)lane;
            this.LaneIndex = laneIndex;
            this.Die = die;
        }
        #endregion

        #region Properties
        public IGameBoardLane Lane { get; }

        IBoardLane IMove.Lane => this.Lane;

        public int LaneIndex { get; }

        public IDie Die { get; }
        #endregion

        #region Public and overriden methods
        public override bool Equals(object obj)
        {
            var other = obj as IGameMove;
            if (other != null)
                return this.LaneIndex == other.LaneIndex && this.Die.Equals(other.Die);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Die.GetHashCode() + this.LaneIndex.GetHashCode();
        }
        #endregion
    }
}
