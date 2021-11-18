// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using AppBrix.Backgammon.Game;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Rules;

public interface IRules
{
    IEnumerable<IMove> GetAvailableMoves(IBoard board, ITurn turn);
}
