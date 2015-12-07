// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Game;
using AppBrix.Events;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Events
{
    /// <summary>
    /// A base interface for game related events.
    /// </summary>
    public interface IGameEvent : IEvent
    {
        /// <summary>
        /// Gets the current game.
        /// </summary>
        IGame Game { get; }
    }
}
