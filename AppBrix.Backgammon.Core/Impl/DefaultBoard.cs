// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultBoard : IGameBoard
    {
        #region Construction
        public DefaultBoard(IReadOnlyList<IGameBoardLane> lanes)
        {
            if (lanes == null)
                throw new ArgumentNullException("lanes");

            this.GameLanes = lanes;
        }

        #endregion

        #region Properties
        public IReadOnlyList<IGameBoardLane> GameLanes { get; private set; }

        public IReadOnlyList<IBoardLane> Lanes { get { return this.GameLanes; } }
        #endregion
    }
}
