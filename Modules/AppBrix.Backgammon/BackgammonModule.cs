// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Game.Impl;
using AppBrix.Events;
using AppBrix.Factory;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using AppBrix.Random;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon;

/// <summary>
/// A module used for registering commonly used Backgammon functionality.
/// Registers <see cref="Game.IGameFactory"/> and <see cref="Game.IDiceRoller"/>.
/// </summary>
public class BackgammonModule : ModuleBase
{
    #region Properties
    public override IEnumerable<Type> Dependencies => [typeof(EventsModule), typeof(FactoryModule), typeof(RandomModule)];
    #endregion

    #region ModuleBase implementation
    protected override void Initialize(IInitializeContext context)
    {
        this.App.Container.Register(this);
        this.factory.Value.Initialize(context);
        this.App.Container.Register(this.factory.Value);
        this.diceRoller.Value.Initialize(context);
        this.App.Container.Register(this.diceRoller.Value);
    }

    protected override void Uninitialize()
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
