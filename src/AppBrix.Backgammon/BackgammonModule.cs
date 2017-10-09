﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Modules;
using AppBrix.Lifecycle;
using AppBrix.Backgammon.Game.Impl;
using System;
using System.Linq;
using AppBrix.Backgammon.Game;

namespace AppBrix.Backgammon
{
    /// <summary>
    /// A module used for registering commonly used Backgammon functionality.
    /// Registers <see cref="Game.IGameFactory"/> and <see cref="Game.IDiceRoller"/>.
    /// </summary>
    public class BackgammonModule : ModuleBase
    {
        #region ModuleBase implementation
        protected override void InitializeModule(IInitializeContext context)
        {
            this.App.Container.Register(this);
            this.factory.Value.Initialize(context);
            this.App.Container.Register(this.factory.Value);
            this.diceRoller.Value.Initialize(context);
            this.App.Container.Register(this.diceRoller.Value);
        }

        protected override void UninitializeModule()
        {
            this.factory.Value.Uninitialize();
            this.diceRoller.Value.Uninitialize();
        }
        #endregion

        #region Private fields and constants
        private readonly Lazy<DefaultGameFactory> factory = new Lazy<DefaultGameFactory>();
        private readonly Lazy<DefaultDiceRoller> diceRoller = new Lazy<DefaultDiceRoller>();
        #endregion
    }
}
