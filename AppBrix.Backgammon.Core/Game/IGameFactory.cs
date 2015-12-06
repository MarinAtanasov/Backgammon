// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    /// <summary>
    /// Factory used to create <see cref="IPlayer"/> and <see cref="IGame"/> objects.
    /// </summary>
    public interface IGameFactory
    {
        /// <summary>
        /// Creates a new player with a specified name and id.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="id">The id of the player.</param>
        /// <returns>The player.</returns>
        IPlayer CreatePlayer(string name, Guid id);

        /// <summary>
        /// Creates a new game with the selected players.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <returns>The newly created game.</returns>
        IGame CreateGame(IReadOnlyList<IPlayer> players);
    }
}
