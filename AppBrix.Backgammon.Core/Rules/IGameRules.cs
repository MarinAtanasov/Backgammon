﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using AppBrix.Backgammon.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules
{
    internal interface IGameRules : IRules
    {
        void MovePiece(IPlayer player, IGameBoard board, IGameMove move);

        bool CanMakeMove(IPlayer player, IBoard board, IMove move);

        IPlayer TryGetWinner(IBoard board, IMove move, IEnumerable<IPlayer> players);
    }
}
