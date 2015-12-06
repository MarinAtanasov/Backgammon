// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// Represents a turn during the game.
    /// </summary>
    public interface ITurn
    {
        #region Properties
        /// <summary>
        /// Gets whether the dice should be rolled.
        /// </summary>
        bool AreDiceRolled { get; }

        /// <summary>
        /// Gets the rolled dice.
        /// </summary>
        IReadOnlyList<IDie> Dice { get; }

        /// <summary>
        /// Gets the name of the player who will play this turn.
        /// </summary>
        string Player { get; }
        #endregion
    }
}
