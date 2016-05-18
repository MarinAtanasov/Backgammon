// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Board
{
    /// <summary>
    /// A piece which is used while playing Backgammon.
    /// </summary>
    public interface IPiece
    {
        #region Properties
        /// <summary>
        /// Gets the player who owns the piece.
        /// </summary>
        string Player { get; }
        #endregion
    }
}
