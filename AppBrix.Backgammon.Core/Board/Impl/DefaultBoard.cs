// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board.Impl
{
    internal class DefaultBoard : IGameBoard
    {
        #region Construction
        public DefaultBoard(IReadOnlyList<IGameBoardLane> lanes, IGameBoardLane bar, IGameBoardLane bearedOff)
        {
            if (lanes == null)
                throw new ArgumentNullException("lanes");

            this.Bar = bar;
            this.BearedOff = bearedOff;
            this.Lanes = lanes;
        }

        #endregion

        #region Game Properties
        public IGameBoardLane Bar { get; private set; }

        IBoardLane IBoard.Bar { get { return this.Bar; } }

        public IGameBoardLane BearedOff { get; private set; }

        IBoardLane IBoard.BearedOff { get { return this.BearedOff; } }

        public IReadOnlyList<IGameBoardLane> Lanes { get; private set; }

        IReadOnlyList<IBoardLane> IBoard.Lanes { get { return this.Lanes; } }
        #endregion
    }
}
