// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultBoard : IBoard
    {
        #region Construction
        public DefaultBoard(IReadOnlyList<IBoardLane> lanes)
        {
            if (lanes == null)
                throw new ArgumentNullException("lanes");

            this.Lanes = lanes;
        }
        #endregion

        #region Properties
        public IReadOnlyList<IBoardLane> Lanes { get; private set; }
        #endregion
    }
}
