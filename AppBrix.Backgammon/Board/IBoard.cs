// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Board
{
    /// <summary>
    /// The Backgammon board.
    /// </summary>
    public interface IBoard
    {
        #region Properties
        /// <summary>
        /// Gets the Backgammon bar. Holds pieces which must be entered on the board's lanes.
        /// </summary>
        IBoardLane Bar { get; }

        /// <summary>
        /// Gets the pieces which have been beared off.
        /// </summary>
        IBoardLane BearedOff { get; }

        /// <summary>
        /// Gets the board's lanes.
        /// </summary>
        IReadOnlyList<IBoardLane> Lanes { get; }
        #endregion
    }
}
