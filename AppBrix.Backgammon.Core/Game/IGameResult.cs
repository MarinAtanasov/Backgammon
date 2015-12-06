// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// Used by the <see cref="IGame.GameFinished"/> event.
    /// </summary>
    public interface IGameResult
    {
        /// <summary>
        /// Gets the name of the player who has won the game.
        /// </summary>
        string Winner { get; }
    }
}
