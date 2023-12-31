﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using iLegMusic.ViewModels.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace iLegMusic.Views.Windows;

public partial class MainWindow : UiWindow
{
    [STAThread]
    [DllImport("Kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSize, int maximumWorkingSetSize);

    MainWindowViewModel? _vm;
    public MainWindow(
    )
    {

       
        InitializeComponent();
        Watcher.Watch(this);
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
            _vm.IsFinishPlay = true;
            if (_vm.Iconrepeat == Wpf.Ui.Common.SymbolRegular.ArrowRepeatAll20)
                _vm.NextCommand.Execute(null);
            else if (_vm.Iconrepeat == Wpf.Ui.Common.SymbolRegular.ArrowRepeat120)
                _vm.PlayOrPauseCommand.Execute(null);
            else if (_vm.MusicSelected == null)
                _vm.Musics?.Where(x => x.IsInPlay == true).ToList().ForEach(x => x.IsInPlay = false);
        }
    }

    private void media_MediaOpened(object sender, RoutedEventArgs e)
    {
        if (_vm != null && _vm.Musics != null)
        {
            _vm.IsFinishPlay = false;
            _vm.Symbolplay = Wpf.Ui.Common.SymbolRegular.Pause20;               
            _vm.Musics.Where(x=> x.IsInPlay == true).ToList().ForEach(x => x.IsInPlay = false);
            _vm.Musics.First(x => x == _vm.MusicSelected).IsInPlay = true;
            if (media.NaturalDuration.HasTimeSpan)
            {
                _vm.MaxValueMusicTime = media.NaturalDuration.TimeSpan.TotalSeconds;
                _vm.InitTask();
            }
        }
    }
    public void Alzeimer()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }

    private void media_MediaFailed(object sender, ExceptionRoutedEventArgs e)
    {
        System.Windows.MessageBox.Show(e.ErrorException.Message);
    }
}
