// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl.Rules
{
    internal sealed class MoveRuleContext
    {
        public MoveRuleContext()
        {
            this.Moves = new Dictionary<IBoardLane, ISet<IDie>>();
        }

        public bool HasMoves { get; private set; }

        public IDictionary<IBoardLane, ISet<IDie>> Moves { get; private set; }

        public void AddMove(IBoardLane lane, IDie die)
        {
            this.HasMoves = true;
            if (!this.Moves.ContainsKey(lane))
                this.Moves.Add(lane, new HashSet<IDie>());

            if (!this.Moves[lane].Contains(die))
                this.Moves[lane].Add(die);
        }

        public bool CanMove(IBoardLane lane, IDie die)
        {
            return this.Moves.ContainsKey(lane) && this.Moves[lane].Contains(die);
        }
    }
}
