// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using System;
using System.Threading;

namespace AppBrix.Backgammon.Game.Impl
{
    internal class DefaultDiceRoller : IDiceRoller, IApplicationLifecycle
    {
        #region Public and overriden metheods
        public void Initialize(IInitializeContext context)
        {
            this.app = context.App;
        }

        public void Uninitialize()
        {
            this.app = null;
        }

        public int RollDie() => this.app.GetRandomService().GetRandom().Next(1, 7);
        #endregion

        #region Private fields and constants
        #nullable disable
        private IApp app;
        #nullable restore
        #endregion
    }
}
