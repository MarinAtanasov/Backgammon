// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Board.Impl
{
    internal interface IGameBoard : IBoard
    {
        #region Properties
        new IGameBoardLane Bar { get; }

        new IGameBoardLane BearedOff { get; }

        new IReadOnlyList<IGameBoardLane> Lanes { get; }
        #endregion
    }
}
