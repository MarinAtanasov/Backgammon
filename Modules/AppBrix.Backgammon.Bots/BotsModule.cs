// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using AppBrix.Modules;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.Bots;

/// <summary>
/// A module used for registering Backgammon bots.
/// </summary>
public class BotsModule : ModuleBase
{
    #region Properties
    public override IEnumerable<Type> Dependencies => [typeof(BackgammonModule)];
    #endregion

    #region ModuleBase implementation
    protected override void Initialize(IInitializeContext context)
    {
        this.App.Container.Register(new DefaultBotRegister(this.App));
    }
    #endregion
}
