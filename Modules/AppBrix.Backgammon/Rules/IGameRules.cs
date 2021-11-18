// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Board.Impl;
using AppBrix.Backgammon.Game;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Rules;

internal interface IGameRules : IRules
{
    void MovePiece(IPlayer player, IGameBoard board, IGameMove move);

    bool CanMakeMove(IPlayer player, IBoard board, IMove move);

    IPlayer? TryGetWinner(IBoard board, IMove move, IEnumerable<IPlayer> players);
}
