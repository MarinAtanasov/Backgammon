// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board
{
    /// <summary>
    /// A lane on the Backgammon board.
    /// Every lane can hold <see cref="IPiece"/>.
    /// </summary>
    public interface IBoardLane
    {
        #region Properties
        /// <summary>
        /// Gets a list of <see cref="IPiece"/> elements on the lane.
        /// </summary>
        IReadOnlyList<IPiece> Pieces { get; }

        /// <summary>
        /// Gets the top <see cref="IPiece"/>. Returns null if there are none.
        /// </summary>
        IPiece TopPiece { get; }
        #endregion
    }
}
