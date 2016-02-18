// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using AppBrix.Backgammon.Core.Board.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules
{
    internal interface IGameRules
    {
        IReadOnlyCollection<IMove> GetValidMoves(IGameBoard board, ITurn turn);

        void MovePiece(IPlayer player, IGameBoard board, IGameMove move);

        IPlayer TryGetWinner(IBoard board, IEnumerable<IPlayer> players);
    }
}
