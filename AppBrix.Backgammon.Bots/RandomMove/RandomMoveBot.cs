// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Game;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Bots.RandomMove
{
    public class RandomMoveBot : BotBase
    {
        public RandomMoveBot(IPlayer player)
            : base(player)
        {
        }

        protected override void MakeMove(IGame game)
        {
            var move = game.AllowedMoves.Skip(RandomMoveBot.random.Next(game.AllowedMoves.Count)).First();
            game.PlayMove(this.Player, move);
        }

        #region Private fields and constants
        private static Random random = new Random();
        #endregion
    }
}
