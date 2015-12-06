// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl
{
    internal class DefaultGameResult : IGameResult
    {
        #region Construction
        public DefaultGameResult(IPlayer winner)
        {
            this.Winner = winner.Name;
        }
        #endregion

        #region Properties
        public string Winner { get; private set; }
        #endregion
    }
}
