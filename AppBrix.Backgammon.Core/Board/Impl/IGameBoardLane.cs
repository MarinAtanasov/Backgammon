// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board.Impl
{
    internal interface IGameBoardLane : IBoardLane
    {
        #region Methods
        void AddPiece(IPiece piece);

        void RemovePiece(IPiece piece);
        #endregion
    }
}
