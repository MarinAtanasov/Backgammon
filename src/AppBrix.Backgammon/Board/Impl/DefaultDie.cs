// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;

namespace AppBrix.Backgammon.Board.Impl
{
    internal class DefaultDie : IDie
    {
        #region Construction
        public DefaultDie(bool isUsed, int value)
        {
            if (value < 1 || value > 6)
                throw new ArgumentException("Die value should be between 1 and 6. Provided: " + value);

            this.IsUsed = isUsed;
            this.Value = value;
        }
        #endregion

        #region Properties
        public bool IsUsed { get; }

        public int Value { get; }
        #endregion

        #region Public and overriden methods
        public override bool Equals(object obj)
        {
            var other = obj as DefaultDie;
            if (other != null)
                return this.IsUsed == other.IsUsed && this.Value == other.Value;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
        #endregion
    }
}
