﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using StockXpertise.Models;
using StockXpertise.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace StockXpertise.Views.Windows
{
    public partial class MainWindow
    {
        public MainWindowViewModel? ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            navigationService.SetNavigationControl(NavigationView);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            NavigationView.SetServiceProvider(serviceProvider);

            var produits = Produit.GetAll();

            foreach (Produit produit in produits)
            {
                Console.WriteLine(produit.id_articles.ResolveEntity().id_fournisseur.ResolveEntity().nom);
            }
        }
    }
}
