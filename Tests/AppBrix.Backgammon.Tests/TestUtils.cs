﻿// Copyright (c) MarinAtanasov. All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the project root for license information.
//
using AppBrix.Configuration;
using AppBrix.Configuration.Memory;
using AppBrix.Modules;
using FluentAssertions;
using System;
using System.Linq;

namespace AppBrix.Backgammon.Tests;

/// <summary>
/// Contains commonly used testing utilities.
/// </summary>
public static class TestUtils
{
    #region Public and overriden methods
    /// <summary>
    /// Creates an app with an in-memory configuration using the provided module and its dependencies.
    /// </summary>
    /// <param name="modules">The modules to load inside the application.</param>
    /// <returns>The created application.</returns>
    public static IApp CreateTestApp(params Type[] modules)
    {
        IConfigService service = new MemoryConfigService();
        var config = service.Get<AppConfig>();
        modules.SelectMany(module => module.CreateObject<IModule>().GetAllDependencies())
            .Concat(modules)
            .Distinct()
            .Select(ModuleConfigElement.Create)
            .ToList()
            .ForEach(config.Modules.Add);
        return App.Create(service);
    }

    public static void TestPerformance(Action action)
    {
        // Invoke the action once to make sure that the assemblies are loaded.
        action();

        action.ExecutionTime().Should().BeLessThan(TimeSpan.FromMilliseconds(100), "this is a performance test");
    }
    #endregion
}
