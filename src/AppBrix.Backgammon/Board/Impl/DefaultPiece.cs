// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;
using System;

namespace AppBrix.Backgammon.Board.Impl;

internal class DefaultPiece : IPiece
{
    #region Construction
    public DefaultPiece(IPlayer player)
    {
        if (player is null)
            throw new ArgumentNullException(nameof(player));

        this.Player = player.Name;
    }
    #endregion

    #region Properties
    public string Player { get; }
    #endregion

    #region Public and overriden methods
    public override string ToString()
    {
        return this.Player;
    }
    #endregion
}
