// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using iLegMusic.Models;
using iLegMusic.ViewModels.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace iLegMusic.Views.Pages
{
    public partial class DashboardPage : Page
    {
        MainWindowViewModel? _vm;
        public DashboardPage()
        {
            InitializeComponent();
            _vm = App.GetService<MainWindowViewModel>();    
            DataContext= _vm;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(_vm != null)
            {
                var btn = (Wpf.Ui.Controls.Button)sender;
                var context = btn.DataContext as MusicModel;
                if(context != null)
                {
                    _vm.MusicSelected = context;
                }
            }
        }
    }
}
