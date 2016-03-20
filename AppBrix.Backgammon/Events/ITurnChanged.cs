// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Events
{
    /// <summary>
    /// An event which is raised after every player action.
    /// </summary>
    public interface ITurnChanged : IGameEvent
    {
    }
}
