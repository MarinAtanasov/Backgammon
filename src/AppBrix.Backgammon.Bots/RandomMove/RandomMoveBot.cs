// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.Bots.RandomMove
{
    public class RandomMoveBot : BotBase
    {
        public RandomMoveBot(IPlayer player)
            : base(player)
        {
        }

        protected override void MakeMove(IGame game, IReadOnlyList<IMove> moves)
        {
            game.PlayMove(this.Player, moves[RandomMoveBot.random.Next(moves.Count)]);
        }

        #region Private fields and constants
        private static readonly Random random = new Random();
        #endregion
    }
}
