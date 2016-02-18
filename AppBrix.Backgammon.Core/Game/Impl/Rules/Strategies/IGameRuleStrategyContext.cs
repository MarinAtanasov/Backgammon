// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game.Impl.Rules.Strategies
{
    /// <summary>
    /// Context used by game rule strategies.
    /// </summary>
    internal interface IGameRuleStrategyContext
    {
        /// <summary>
        /// Gets or sets whether the strategy has found all available moves.
        /// </summary>
        bool IsDone { get; set; }

        /// <summary>
        /// Gets the available moves.
        /// </summary>
        List<IGameMove> Moves { get; }
    }
}
