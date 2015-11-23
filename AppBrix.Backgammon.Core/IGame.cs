// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core
{
    public interface IGame
    {
        #region Properties
        ITurn Turn { get; }

        IReadOnlyCollection<IPlayer> Players { get; }
        #endregion

        #region Methods
        ITurn RollDice(IPlayer player);

        ITurn PlayDie(IPlayer player, IBoardLane lane, IDie die);
        #endregion
    }
}
