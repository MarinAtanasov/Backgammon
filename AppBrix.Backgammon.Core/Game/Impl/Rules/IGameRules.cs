// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules
{
    internal interface IGameRules
    {
        bool IsMoveValid(IPlayer player, IBoard board, IBoardLane lane, IDie die);

        void MovePiece(IPlayer player, IGameBoard board, IGameBoardLane lane, IDie die);
    }
}
