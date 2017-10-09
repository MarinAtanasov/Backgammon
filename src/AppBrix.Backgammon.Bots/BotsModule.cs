// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Modules;
using AppBrix.Lifecycle;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Bots
{
    /// <summary>
    /// A module used for registering Backgammon bots.
    /// </summary>
    public class BotsModule : ModuleBase
    {
        #region ModuleBase implementation
        protected override void InitializeModule(IInitializeContext context)
        {
            this.App.Container.Register(this);
            this.App.Container.Register(new DefaultBotRegister(this.App));
        }

        protected override void UninitializeModule()
        {
        }
        #endregion
    }
}
