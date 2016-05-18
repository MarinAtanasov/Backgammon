// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Bots
{
    /// <summary>
    /// Describes a backgammon game bot.
    /// </summary>
    public interface IBot
    {
        #region Properties
        /// <summary>
        /// Gets or sets the player whose turn the bot will control.
        /// </summary>
        IPlayer Player { get; }
        #endregion

        #region Public methods
        /// <summary>
        /// Executes when a game's turn is changed.
        /// </summary>
        /// <param name="game">The game.</param>
        void PlayTurn(IGame game);
        #endregion
    }
}
