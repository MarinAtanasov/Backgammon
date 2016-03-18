// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board.Impl
{
    internal class DefaultPiece : IPiece
    {
        #region Construction
        public DefaultPiece(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            this.Player = player.Name;
        }
        #endregion

        #region Properties
        public string Player { get; private set; }
        #endregion

        #region Public and overriden methods
        public override string ToString()
        {
            return this.Player;
        }
        #endregion
    }
}
