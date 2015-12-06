﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGameEnded : IGameEnded
    {
        #region Construction
        public DefaultGameEnded(IGame game)
        {
            this.Game = game;
        }
        #endregion

        #region Properties
        public IGame Game { get; private set; }
        #endregion
    }
}
