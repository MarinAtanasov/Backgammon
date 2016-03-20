// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Rules.Strategies
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
        #endregion

        #region Private fields and constants
        private bool isDone;
        #endregion
    }
}
