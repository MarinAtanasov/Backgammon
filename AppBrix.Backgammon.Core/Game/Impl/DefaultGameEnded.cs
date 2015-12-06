// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGameEnded : IGameEnded
    {
        #region Construction
        public DefaultGameEnded(IGame game, IPlayer winner)
        {
            this.Game = game;
            this.Winner = winner.Name;
        }
        #endregion

        #region Properties
        public IGame Game { get; private set; }

        public string Winner { get; private set; }
        #endregion
    }
}
