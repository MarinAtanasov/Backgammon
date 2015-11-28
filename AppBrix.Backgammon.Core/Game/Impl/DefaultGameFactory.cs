// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGameFactory : IGameFactory
    {
        #region Construction
        public DefaultGameFactory(IApp app)
        {
            this.app = app;
        }
        #endregion

        #region Public and overriden methods
        public IPlayer CreatePlayer(string name, Guid id)
        {
            return new DefaultPlayer(name, id);
        }

        public IGame CreateGame(IReadOnlyList<IPlayer> players)
        {
            if (players.Count != 2)
                throw new ArgumentException("There should be exactly 2 players. Found: " + players.Count);

            return new DefaultGame(this.app, players[0], players[1]);
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
