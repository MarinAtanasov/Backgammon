// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Events;
using AppBrix.Backgammon.Game;
using System;

namespace AppBrix.Backgammon.Bots
{
    internal class DefaultBotRegister : IBotRegister
    {
        #region Construction
        public DefaultBotRegister(IApp app)
        {
            this.App = app;
        }
        #endregion

        #region Properties
        public IApp App { get; }
        #endregion

        #region Public methods
        public void RegisterBot(IGame game, IBot bot)
        {
            Action<ITurnChanged> onTurnChanged = x =>
            {
                if (x.Game == game && !game.HasEnded && game.Turn.Player == bot.Player.Name)
                    bot.PlayTurn(game);
            };
            this.App.GetEventHub().Subscribe(onTurnChanged);

            Action<IGameEnded> onGameEnded = null;
            onGameEnded = x =>
            {
                if (game == x.Game)
                {
                    this.App.GetEventHub().Unsubscribe(onTurnChanged);
                    this.App.GetEventHub().Unsubscribe(onGameEnded);
                }
            };
            this.App.GetEventHub().Subscribe(onGameEnded);
        }
        #endregion
    }
}
