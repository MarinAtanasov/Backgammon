// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Game
{
    internal interface IGameMove : IMove
    {
        new IGameBoardLane Lane { get; }
    }
}
