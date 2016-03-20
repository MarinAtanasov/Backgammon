// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Board.Impl
{
    internal class DefaultBoardLane : List<IPiece>, IGameBoardLane
    {
        #region Construction
        public DefaultBoardLane(params IPiece[] pieces)
        {
            if (pieces == null)
                throw new ArgumentNullException(nameof(pieces));

            this.AddRange(pieces);
        }
        #endregion
        
        #region Public and overriden methods
        public void MovePiece(IGameBoardLane target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            var targetLane = target as DefaultBoardLane;
            if (targetLane == null)
                throw new ArgumentException("This method requires a target lane of type: " + typeof(DefaultBoardLane).FullName);

            targetLane.Add(this[this.Count - 1]);
            this.RemoveAt(this.Count - 1);
        }
        #endregion
    }
}
