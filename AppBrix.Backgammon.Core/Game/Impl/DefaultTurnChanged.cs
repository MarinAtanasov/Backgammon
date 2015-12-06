// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultTurnChanged : ITurnChanged
    {
        #region Construction
        public DefaultTurnChanged(IGame game, ITurn turn)
        {
            this.Game = game;
            this.Turn = turn;
        }
        #endregion

        #region Properties
        public IGame Game { get; private set; }

        public ITurn Turn { get; private set; }
        #endregion
    }
}
