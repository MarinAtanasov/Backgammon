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
    public class ConfigInitializerModule : IModule, IInstallable
    {
        #region Public and overriden methods
        public static IApp CreateApp()
        {
            var configService = new MemoryConfigService();
            if (configService.Get<AppConfig>().Modules.Count == 0)
                configService.Get<AppConfig>().Modules.Add(ModuleConfigElement.Create<ConfigInitializerModule>());

            var app = AppBrix.App.Create(configService);
            app.Start();
            return app;
        }

        public void Install(IInstallContext context)
        {
            this.InitializeAppConfig(context.App.ConfigService);
            context.RequestedAction = RequestedAction.Restart;
        }

        public void Upgrade(IUpgradeContext context)
        {
        }

        public void Uninstall(IUninstallContext context)
        {
            throw new NotSupportedException($@"Module {nameof(ConfigInitializerModule)} does not support uninstallation.");
        }

        public void Initialize(IInitializeContext context)
        {
        }

        public void Uninitialize()
        {
        }
        #endregion

        #region Private methods
        private void InitializeAppConfig(IConfigService service)
        {
            var config = service.Get<AppConfig>();
            if (config.Modules.Count > 1)
                throw new InvalidOperationException($@"Module {nameof(ConfigInitializerModule)} found other modules registered besides itself.");

            this.GetType()
                .GetModuleDependencies()
                .Select(ModuleConfigElement.Create)
                .ToList()
                .ForEach(config.Modules.Add);
        }
        #endregion

        #region Private fields and constants
        private static readonly IEnumerable<Type> Modules = new List<Type>()
        {
            typeof(BackgammonModule),
            typeof(BotsModule)
        };
        #endregion
    }
}
