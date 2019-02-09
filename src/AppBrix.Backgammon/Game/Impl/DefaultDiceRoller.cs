// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Lifecycle;
using System;
using System.Linq;
using System.Threading;

namespace AppBrix.Backgammon.Game.Impl
{
    internal class DefaultDiceRoller : IDiceRoller, IApplicationLifecycle, IDisposable
    {
        #region Public and overriden metheods
        public void Initialize(IInitializeContext context)
        {
            this.app = context.App;
            this.app.GetFactoryService().Register(() => new Random());
            this.random = new ThreadLocal<Random>(() => this.app.GetFactoryService().Get<Random>());
        }

        public void Uninitialize()
        {
            ((IDisposable)this).Dispose();
            this.app = null;
            this.random = null;
        }

        void IDisposable.Dispose()
        {
            this.random.Dispose();
        }

        public int RollDie()
        {
            return this.random.Value.Next(1, 7);
        }
        #endregion

        #region Private fields and constants
        private IApp app;
        private ThreadLocal<Random> random;
        #endregion
    }
}
