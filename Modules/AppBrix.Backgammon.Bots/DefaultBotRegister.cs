// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Events;
using AppBrix.Backgammon.Game;
using System;

namespace AppBrix.Backgammon.Bots;

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
        void onTurnChanged(ITurnChanged x)
        {
            if (x.Game == game && !game.HasEnded && game.Turn.Player == bot.Player.Name)
                bot.PlayTurn(game);
        }

        void onGameEnded(IGameEnded x)
        {
            if (game == x.Game)
            {
                this.App.GetEventHub().Unsubscribe((Action<ITurnChanged>)onTurnChanged);
                this.App.GetEventHub().Unsubscribe((Action<IGameEnded>)onGameEnded);
            }
        }

        this.App.GetEventHub().Subscribe((Action<ITurnChanged>)onTurnChanged);
        this.App.GetEventHub().Subscribe((Action<IGameEnded>)onGameEnded);
    }
    #endregion
}
