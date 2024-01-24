﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using StockXpertise.Helpers;
using StockXpertise.Services;
using StockXpertise.ViewModels.Pages;
using StockXpertise.ViewModels.Windows;
using StockXpertise.Views.Pages;
using StockXpertise.Views.Windows;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Threading;



namespace StockXpertise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            AllocConsole();
            base.OnStartup(e);
        }

        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                 c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
             }).ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configuration\", "config.xml");
                DBConfig config = DBConfig.LoadFromXml(path);

                var conn = new MySqlConnection(config.ToString());
                var compiler = new MySqlCompiler();

                var queryBuilder = new QueryFactory(conn, compiler);
                services = services.AddSingleton(queryBuilder);

                // ici c'est la ou la DI enregistre les classes 
                // ( ce que on fait dans login window c'est de la duplication, faut les recup instead of les re-initializer ) (resolve)
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<LoginWindow>();
                services.AddSingleton<LoginViewModel>();

                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                services.AddSingleton<StockPage>();
                services.AddSingleton<StockViewModel>();

                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
             return _host.Services.GetService(typeof(T)) as T;
         }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
