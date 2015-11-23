// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultDiceRoller : IDiceRoller
    {
        #region Construction
        public DefaultDiceRoller(Random random)
        {
            if (random == null)
                throw new ArgumentNullException("random");

            this.random = random;
        }
        #endregion

        #region Public and overriden metheods
        public int RollDie()
        {
            return this.random.Next(1, 7);
        }
        #endregion

        #region Private fields and constants
        private readonly Random random;
        #endregion
    }
}
