// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Backgammon.Bots;
using AppBrix.Modules;
using System;
using System.Collections.Generic;

namespace AppBrix.Backgammon.ConsoleApp;

/// <summary>
/// Main module for the application.
/// </summary>
public class MainModule : MainModuleBase
{
	public override IEnumerable<Type> Dependencies => [typeof(BackgammonModule), typeof(BotsModule)];
}
