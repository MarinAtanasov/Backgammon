﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Events.Impl
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