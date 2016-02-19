// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Rules.Strategies
{
    internal class DefaultGameRuleStrategyContext : IGameRuleStrategyContext
    {
        #region Properties
        public bool IsDone
        {
            get
            {
                return this.isDone;
            }
            set
            {
                if (this.IsDone != value)
                {
                    if (this.IsDone)
                        throw new InvalidOperationException("Once the context is finished, it cannot be resumed.");

                    this.isDone = value;
                }
            }
        }

        public List<IGameMove> Moves { get { return this.moves; } }
        #endregion

        #region Private fields and constants
        private bool isDone;
        private List<IGameMove> moves = new List<IGameMove>();
        #endregion
    }
}
