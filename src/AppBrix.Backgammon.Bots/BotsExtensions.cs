// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Bots;
using AppBrix.Backgammon.Game;

namespace AppBrix.Backgammon
{
    public static class BotsExtensions
    {
        /// <summary>
        /// Gets the currently registered <see cref="IBotRegister"/>.
        /// </summary>
        /// <param name="app">The current application.</param>
        /// <returns>The bot register.</returns>
        public static IBotRegister GetBotRegister(this IApp app) => (IBotRegister)app.Get(typeof(IBotRegister));

        /// <summary>
        /// Gets the currently registered <see cref="IBotRegister"/> and uses it to register the bot inside the game.
        /// </summary>
        /// <param name="game">The game in which the bot will participate.</param>
        /// <param name="bot">The bot.</param>
        public static void RegisterBot(this IGame game, IBot bot) => game.App.GetBotRegister().RegisterBot(game, bot);
    }
}
