// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Bots.RandomMove
{
    public class RandomMoveBot : BotBase
    {
        public RandomMoveBot(IApp app, IPlayer player)
            : base(player)
        {
            this.app = app;
        }

        protected override void MakeMove(IGame game, IReadOnlyList<IMove> moves) =>
            game.PlayMove(this.Player, moves[this.app.GetRandomService().GetRandom().Next(moves.Count)]);

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
