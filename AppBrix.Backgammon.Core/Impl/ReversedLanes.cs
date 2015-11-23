// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    public class ReversedLanes : IReadOnlyList<IBoardLane>
    {
        #region Construction
        public ReversedLanes(IReadOnlyList<IBoardLane> original)
        {
            this.original = original;
        }
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return this.original.Count;
            }
        }

        public IBoardLane this[int index]
        {
            get
            {
                return this.original[this.original.Count - 1 - index];
            }
        }
        #endregion

        #region Public and overriden methods
        public IEnumerator<IBoardLane> GetEnumerator()
        {
            for (int i = this.original.Count - 1; i >= 0; i--)
            {
                yield return original[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Private fields and constants
        private readonly IReadOnlyList<IBoardLane> original;
        #endregion
    }
}
