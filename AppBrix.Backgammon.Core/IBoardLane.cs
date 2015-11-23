// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core
{
    public interface IBoardLane
    {
        #region Properties
        IReadOnlyList<IPiece> Pieces { get; }

        IPiece TopPiece { get; }
        #endregion

        #region Methods
        void AddPiece(IPiece piece);

        void RemovePiece(IPiece piece);
        #endregion
    }
}
