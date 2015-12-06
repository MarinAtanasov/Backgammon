﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Core.Board;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Game
{
    public interface IGame
    {
        #region Properties
        ITurn Turn { get; }
        #endregion

        #region Events
        event Action<ITurn> TurnChanged;

        event Action<IGameResult> GameFinished;
        #endregion

        #region Methods
        IBoard GetBoard(IPlayer player);

        void RollDice(IPlayer player);

        void PlayDie(IPlayer player, IBoardLane lane, IDie die);
        #endregion
    }
}