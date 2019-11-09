// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Application;
using AppBrix.Backgammon.Bots;
using AppBrix.Configuration;
using AppBrix.Configuration.Memory;
using AppBrix.Lifecycle;
using AppBrix.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBrix.Backgammon.ConsoleApp
{
    /// <summary>
    /// Initializes application configuration.
    /// This module should be first on the list in order to configure the application's configuration.
    /// </summary>
    public class ConfigInitializerModule : MainModuleBase
    {
        #region Private fields and constants
        private static readonly IEnumerable<Type> Modules = new List<Type>()
        {
            typeof(BackgammonModule),
            typeof(BotsModule)
        };
        #endregion
    }
}
