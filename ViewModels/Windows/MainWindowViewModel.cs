// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using iLegMusic.Helpers;
using iLegMusic.Models;
using iLegMusic.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace iLegMusic.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {

        LegMusicServiceGlobal? _service;
        public MainWindowViewModel()
        {
            _service = App.GetService<LegMusicServiceGlobal>();
            if (_service != null)
                _service.MusicEventFound += _service_MusicEventFound;
        }

        private void _service_MusicEventFound(object? sender, List<MusicModel> e)
        {
            e.ForEach(x => Musics.Add(x));
            MusicsCount += e.Count();
        }

        [ObservableProperty]
        Visibility _visibleMain = Visibility.Hidden;

        [ObservableProperty]
        Visibility _visibleHub = Visibility.Visible;

        [ObservableProperty]
        int _MusicsCount = 0;

        [ObservableProperty]
        string _user = "C:\\Users";

        [ObservableProperty]
        private string _applicationTitle = "iLegMusic";

        [ObservableProperty]
        MusicModel? _MusicSelected = null;

        [ObservableProperty]
        ObservableCollection<MusicModel> _Musics = new ObservableCollection<MusicModel>();

        [ObservableProperty]
        ObservableCollection<AlbumModel> _Albums = new ObservableCollection<AlbumModel>();

        [ObservableProperty]
        ObservableCollection<ArtistModel> _Artists = new ObservableCollection<ArtistModel>();

        [ObservableProperty]
        SymbolRegular _symbolplay = SymbolRegular.Play20;

        [ObservableProperty]
        Visibility _VisibleIFFind = Visibility.Collapsed;

        [ObservableProperty]
        bool _enabledFinish = true;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home20},
                TargetPageType = typeof(Views.Pages.DashboardPage)
            }
        };

        [ObservableProperty]
        MediaElement? _mediaelement;


        [RelayCommand]
        private void SearchMusics()
        {
            _service?.GetFilesWithSource(() =>
            {
                VisibleIFFind = Visibility.Visible;
                EnabledFinish = false;
            }, () =>
            {
                VisibleIFFind = Visibility.Collapsed;
                EnabledFinish = true;
                VisibleHub = Visibility.Collapsed;
                VisibleMain = Visibility.Visible;
            });
        }

        [RelayCommand]
        void Next() {
            if (MusicSelected != null)
            {
                var index = Musics.IndexOf(MusicSelected);
                if (index != Musics.Count - 1)
                {
                    MusicSelected = Musics[index + 1];
                    Mediaelement?.Play();
                }
            }
        }

        [RelayCommand]
        void Back()
        {
            if (MusicSelected != null)
            {
                var index = Musics.IndexOf(MusicSelected);
                if (index != 0)
                {
                    MusicSelected = Musics[index - 1];
                    Mediaelement?.Play();
                }
            }
        }


        [RelayCommand]
        void PlayOrPause() {
            if (Mediaelement != null)
            {
                if (Symbolplay != SymbolRegular.Play20)
                {
                    Mediaelement.Pause();
                    Symbolplay = SymbolRegular.Play20;
                }
                else
                {
                    if(MusicSelected == null)
                        MusicSelected = Musics[0];
                    Mediaelement.Play();
                    Symbolplay = SymbolRegular.Pause20;
                }
            }
        }

    }
}
