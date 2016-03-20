// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Game.Impl
{
    internal class DefaultTurn : ITurn
    {
        #region Construction
        public DefaultTurn(IPlayer player, IReadOnlyList<IDie> dice)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            this.Player = player.Name;
            this.Dice = dice;
        }
        #endregion

        #region Properties
        public bool AreDiceRolled { get { return this.Dice.Count > 0; } }

        public IReadOnlyList<IDie> Dice { get; private set; }

        public string Player { get; private set; }
        #endregion
    }
}
