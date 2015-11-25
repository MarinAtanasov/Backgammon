// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core
{
    public interface IGame
    {
        #region Properties
        ITurn Turn { get; }
        #endregion

        #region Methods
        IBoard GetBoard(IPlayer player);

        ITurn RollDice(IPlayer player);

        ITurn PlayDie(IPlayer player, IBoardLane lane, IDie die);
        #endregion
    }
}
