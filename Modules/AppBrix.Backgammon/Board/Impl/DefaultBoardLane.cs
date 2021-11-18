// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Board.Impl;

internal class DefaultBoardLane : List<IPiece>, IGameBoardLane
{
    #region Construction
    public DefaultBoardLane(params IPiece[] pieces)
    {
        if (pieces is null)
            throw new ArgumentNullException(nameof(pieces));

        this.AddRange(pieces);
    }
    #endregion

    #region Public and overriden methods
    public void MovePiece(IGameBoardLane target)
    {
        if (target is null)
            throw new ArgumentNullException(nameof(target));
        if (target is not DefaultBoardLane targetLane)
            throw new ArgumentException($"This method requires a target lane of type: {typeof(DefaultBoardLane).FullName}");

        targetLane.Add(this[^1]);
        this.RemoveAt(this.Count - 1);
    }
    #endregion
}
