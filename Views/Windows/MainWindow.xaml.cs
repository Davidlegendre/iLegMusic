﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using iLegMusic.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace iLegMusic.Views.Windows
{
    public partial class MainWindow : FluentWindow
    {
        MainWindowViewModel? _vm;
        public MainWindow(
        )
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            InitializeComponent();
            _vm = DataContext as MainWindowViewModel;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(_vm != null )
            {
                _vm.Mediaelement = media;
            }
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if(_vm!= null) {
                _vm.Symbolplay = Wpf.Ui.Common.SymbolRegular.Play20;
            }
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
            {
                _vm.Symbolplay = Wpf.Ui.Common.SymbolRegular.Pause20;
            }
        }
    }
}