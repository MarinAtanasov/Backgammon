// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Board.Impl
{
    internal class DefaultBoardLane : IGameBoardLane
    {
        #region Construction
        public DefaultBoardLane(params IPiece[] pieces)
        {
            if (pieces == null)
                throw new ArgumentNullException("pieces");

            this.pieces = new List<IPiece>(pieces);
        }
        #endregion

        #region Properties
        public IReadOnlyList<IPiece> Pieces
        {
            get
            {
                return this.pieces;
            }
        }
        #endregion

        #region Public and overriden methods
        public void MovePiece(IGameBoardLane target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var targetLane = target as DefaultBoardLane;
            if (targetLane == null)
                throw new ArgumentException("This method requires a target lane of type: " + typeof(DefaultBoardLane).FullName);

            var piece = this.pieces[this.pieces.Count - 1];
            this.pieces.Remove(piece);
            targetLane.pieces.Add(piece);
        }
        #endregion

        #region Private fields and constants
        private List<IPiece> pieces;
        #endregion
    }
}
