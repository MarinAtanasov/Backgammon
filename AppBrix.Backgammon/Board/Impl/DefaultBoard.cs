// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Board.Impl
{
    internal class DefaultBoard : IGameBoard
    {
        #region Construction
        public DefaultBoard()
        {
            this.Bar = new DefaultBoardLane();
            this.BearedOff = new DefaultBoardLane();

            var lanes = new List<IGameBoardLane>(24);
            for (int i = 0; i < DefaultBoard.LanesCount; i++)
            {
                lanes.Add(new DefaultBoardLane());
            }
            this.Lanes = lanes;
        }

        public DefaultBoard(IReadOnlyList<IGameBoardLane> lanes, IGameBoardLane bar, IGameBoardLane bearedOff)
        {
            if (lanes == null)
                throw new ArgumentNullException(nameof(lanes));
            if (bar == null)
                throw new ArgumentNullException(nameof(bar));
            if (bearedOff == null)
                throw new ArgumentNullException(nameof(bearedOff));

            this.Bar = bar;
            this.BearedOff = bearedOff;
            this.Lanes = lanes;
        }
        #endregion

        #region Game Properties
        public IGameBoardLane Bar { get; }

        IBoardLane IBoard.Bar => this.Bar;

        public IGameBoardLane BearedOff { get; }

        IBoardLane IBoard.BearedOff => this.BearedOff;

        public IReadOnlyList<IGameBoardLane> Lanes { get; }

        IReadOnlyList<IBoardLane> IBoard.Lanes => this.Lanes;
        #endregion

        #region Private fields and constants
        private const int LanesCount = 24;
        #endregion
    }
}
