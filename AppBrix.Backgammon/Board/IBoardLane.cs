// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Board
{
    /// <summary>
    /// A lane on the Backgammon board.
    /// Every lane can hold <see cref="IPiece"/>.
    /// </summary>
    public interface IBoardLane : IReadOnlyList<IPiece>
    {
    }
}
