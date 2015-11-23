﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultTurn : ITurn
    {
        #region Construction
        public DefaultTurn(IPlayer player, IReadOnlyList<IDie> dice)
        {
            if (player == null)
                throw new ArgumentNullException("player");
            if (dice == null)
                throw new ArgumentNullException("dice");

            this.Player = player;
            this.Dice = dice;
            this.AreDiceRolled = this.Dice.Any();
        }
        #endregion

        #region Properties
        public bool AreDiceRolled { get; private set; }

        public IReadOnlyList<IDie> Dice { get; private set; }

        public IPlayer Player { get; private set; }
        #endregion
    }
}
