// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Events.Impl
{
    internal class DefaultTurnChanged : ITurnChanged
    {
        #region Construction
        public DefaultTurnChanged(IGame game)
        {
            this.Game = game;
        }
        #endregion

        #region Properties
        public IGame Game { get; private set; }
        #endregion
    }
}
