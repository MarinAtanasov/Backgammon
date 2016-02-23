// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Core.Events;
using AppBrix.Backgammon.Core.Game;
using System;
using System.Linq;

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
        public IApp App { get; private set; }
        #endregion

        #region Public methods
        public void RegisterBot(IGame game, IBot bot)
        {
            Action<ITurnChanged> onTurnChanged = (x) => this.OnTurnChanged(game, bot, x);
            this.App.GetEventHub().Subscribe(onTurnChanged);

            Action<IGameEnded> onGameEnded = null;
            onGameEnded = (x) =>
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

        #region Private methods
        private void OnTurnChanged(IGame game, IBot bot, ITurnChanged turnChanged)
        {
            if (turnChanged.Game == game && game.IsRunning && game.Turn.Player == bot.Player.Name)
            {
                bot.PlayTurn(turnChanged.Game);
            }
        }
        #endregion
    }
}
