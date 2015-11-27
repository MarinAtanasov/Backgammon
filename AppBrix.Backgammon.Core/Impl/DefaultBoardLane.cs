// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
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

        public IPiece TopPiece
        {
            get
            {
                return this.pieces.Count > 0 ? this.pieces[this.pieces.Count - 1] : null;
            }
        }
        #endregion

        #region Public and overriden methods
        public void AddPiece(IPiece piece)
        {
            this.pieces.Add(piece);
        }

        public void RemovePiece(IPiece piece)
        {
            this.pieces.Remove(piece);
        }
        #endregion

        #region Private fields and constants
        private List<IPiece> pieces;
        #endregion
    }
}
