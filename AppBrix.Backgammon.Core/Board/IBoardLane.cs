﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board
{
    public interface IBoardLane
    {
        #region Properties
        IReadOnlyList<IPiece> Pieces { get; }

        IPiece TopPiece { get; }
        #endregion
    }
}