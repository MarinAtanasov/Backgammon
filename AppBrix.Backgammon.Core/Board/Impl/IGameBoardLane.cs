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
        /// <summary>
        /// Moves a piece from the current lane to the target lane.
        /// </summary>
        /// <param name="target">The target lane which will receive the piece.</param>
        void MovePiece(IGameBoardLane target);
        #endregion
    }
}
