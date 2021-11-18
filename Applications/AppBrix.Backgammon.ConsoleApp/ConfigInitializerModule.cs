// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Bots;
using AppBrix.Modules;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.ConsoleApp;

/// <summary>
/// Initializes application configuration.
/// This module should be first on the list in order to configure the application's configuration.
/// </summary>
public class ConfigInitializerModule : MainModuleBase
{
    public override IEnumerable<Type> Dependencies => new[] { typeof(BackgammonModule), typeof(BotsModule) };
}
