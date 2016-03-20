// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Game;
using System;
using System.Linq;

namespace AppBrix
{
    public static class BackgammonCoreExtensions
    {
        /// <summary>
        /// Gets the currently registered <see cref="IGameFactory"/>.
        /// </summary>
        /// <param name="app">The current application.</param>
        /// <returns>The game factory.</returns>
        public static IGameFactory GetGameFactory(this IApp app)
        {
            return app.Get<IGameFactory>();
        }

        /// <summary>
        /// Gets the currently registered dice roller.
        /// </summary>
        /// <param name="app">The current application.</param>
        /// <returns>The dice roller.</returns>
        public static IDiceRoller GetDiceRoller(this IApp app)
        {
            return app.Get<IDiceRoller>();
        }

        /// <summary>
        /// Creates a new player with a randomly generated id.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <returns>The new player.</returns>
        public static IPlayer CreatePlayer(this IGameFactory factory, string name)
        {
            return factory.CreatePlayer(name, Guid.Empty);
        }
    }
}
