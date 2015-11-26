// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Core.Impl
{
    internal class DefaultDiceRoller : IDiceRoller
    {
        #region Construction
        public DefaultDiceRoller(IApp app)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            this.app = app;
        }
        #endregion

        #region Public and overriden metheods
        public int RollDie()
        {
            return this.app.GetFactory().Get<Random>().Next(1, 7);
        }
        #endregion

        #region Private fields and constants
        private readonly IApp app;
        #endregion
    }
}
