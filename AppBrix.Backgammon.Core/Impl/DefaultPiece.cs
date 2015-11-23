// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultPiece : IPiece
    {
        #region Construction
        public DefaultPiece(IPlayer owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            this.Owner = owner;
        }
        #endregion

        #region Properties
        public IPlayer Owner { get; private set; }
        #endregion
    }
}
