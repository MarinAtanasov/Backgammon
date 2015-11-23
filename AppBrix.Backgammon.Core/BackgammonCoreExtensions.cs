// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core;
using System;
using System.Linq;

namespace AppBrix
{
    public static class BackgammonCoreExtensions
    {
        /// <summary>
        /// Gets the currently registered dice roller.
        /// </summary>
        /// <param name="app">The current application.</param>
        /// <returns>The dice roller.</returns>
        public static IDiceRoller GetDiceRoller(this IApp app)
        {
            return app.Get<IDiceRoller>();
        }
    }
}
