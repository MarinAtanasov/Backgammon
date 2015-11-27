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
        public DefaultBoard(IReadOnlyList<IGameBoardLane> lanes, IGameBoardLane bar, IGameBoardLane bearedOff)
        {
            if (lanes == null)
                throw new ArgumentNullException("lanes");

            this.GameBar = bar;
            this.GameBearedOff = bearedOff;
            this.GameLanes = lanes;
        }

        #endregion

        #region Game Properties
        public IGameBoardLane GameBar { get; private set; }

        public IGameBoardLane GameBearedOff { get; private set; }

        public IReadOnlyList<IGameBoardLane> GameLanes { get; private set; }
        #endregion

        #region Properties
        public IBoardLane Bar { get { return this.GameBar; } }

        public IBoardLane BearedOff { get { return this.GameBearedOff; } }

        public IReadOnlyList<IBoardLane> Lanes { get { return this.GameLanes; } }
        #endregion
    }
}
