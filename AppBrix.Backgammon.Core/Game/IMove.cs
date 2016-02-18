// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// A move inside the Backgammon game.
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// Gets the lane containing the board piece to be moved.
        /// </summary>
        IBoardLane Lane { get; }

        /// <summary>
        /// Gets the lane index. Returns -1 for board bar.
        /// </summary>
        int LaneIndex { get; }

        /// <summary>
        /// Gets the die to be played.
        /// </summary>
        IDie Die { get; }
    }
}
