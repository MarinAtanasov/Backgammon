// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultDie : IDie
    {
        #region Construction
        public DefaultDie(bool isUsed, int value)
        {
            if (value < 1 || value > 6)
                throw new ArgumentException("Invalid die value: " + value);

            this.IsUsed = isUsed;
            this.Value = value;
        }
        #endregion

        #region Properties
        public bool IsUsed { get; private set; }

        public int Value { get; private set; }
        #endregion

        #region Public and overriden methods
        public override string ToString()
        {
            return this.Value.ToString();
        }
        #endregion
    }
}
