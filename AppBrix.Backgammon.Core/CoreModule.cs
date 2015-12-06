// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Modules;
using System;
using System.Linq;
using AppBrix.Lifecycle;
using AppBrix.Backgammon.Core.Game.Impl;

namespace AppBrix.Backgammon.Core
{
    /// <summary>
    /// A module used for registering commonly used Backgammon functionality.
    /// Registers <see cref="Game.IGameFactory"/> and <see cref="Game.IDiceRoller"/>.
    /// </summary>
    public class CoreModule : ModuleBase
    {
        #region ModuleBase implementation
        protected override void InitializeModule(IInitializeContext context)
        {
            this.App.GetResolver().Register(this);
            this.App.GetResolver().Register(new DefaultGameFactory(this.App));
            this.App.GetResolver().Register(new DefaultDiceRoller(this.App));
        }

        protected override void UninitializeModule()
        {
        }
        #endregion
    }
}
