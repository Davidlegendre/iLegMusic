// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using iLegMusic.Helpers;
using iLegMusic.Models;
using iLegMusic.Services;
using iLegMusic.Views.Windows;
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

        Task? timeTask;

        public void InitTask() {
            if (timeTask == null)
            {
                timeTask = new Task(() =>
                {
                    while (true)
                    {

                        if (Symbolplay != SymbolRegular.Play20 && Mediaelement != null)
                        {
                            App.Current.Dispatcher.Invoke(() => {
                                PositionVlue = Mediaelement.Position;
                                ActualvalueMusicTime = PositionVlue.TotalSeconds;
                            });
                        }
                        Thread.Sleep(1000);
                    }
                });
                timeTask.Start();
            }
        }

        private void _service_MusicEventFound(object? sender, List<MusicModel> e)
        {
            e.ForEach(x => Musics.Add(x));
            MusicsCount += e.Count();
        }

        [ObservableProperty]
        int _colums = 4;

        [ObservableProperty]
        double _volumen = 0.5;

        [ObservableProperty]
        Visibility _visibleMain = Visibility.Collapsed;

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
        Visibility _visibleProtectorWindow = Visibility.Collapsed;

        [ObservableProperty]
        ObservableCollection<GroupMusic> _GrupoMusic = new ObservableCollection<GroupMusic>();

        [ObservableProperty]
        ObservableCollection<MusicModel>? _Musics = new ObservableCollection<MusicModel>();

        [ObservableProperty]
        ObservableCollection<AlbumModel> _Albums = new ObservableCollection<AlbumModel>();

        [ObservableProperty]
        ObservableCollection<ArtistModel> _Artists = new ObservableCollection<ArtistModel>();

        [ObservableProperty]
        MenuOrderModel? _MenuOrderSelected = null;

        [ObservableProperty]
        Visibility _visibleAlbums = Visibility.Collapsed;

        [ObservableProperty]
        Visibility _visibleArtists = Visibility.Collapsed;

        [ObservableProperty]
        Visibility _visibleMusics = Visibility.Visible;

        [ObservableProperty]
        ObservableCollection<MenuOrderModel> _menuOrder = new ObservableCollection<MenuOrderModel>() {
            new(){ Title ="Orden Por Nombre", TypeOrden = TypeOrden.Nombre },
            new() { Title = "Orden Por Album", TypeOrden = TypeOrden.Album },
            new() {Title = "Orden Por Artista", TypeOrden = TypeOrden.Artista}
        };

        [ObservableProperty]
        SymbolRegular _symbolplay = SymbolRegular.Play20;

        [ObservableProperty]
        Visibility _VisibleIFFind = Visibility.Collapsed;

        [ObservableProperty]
        double _maxValueMusicTime = 0;

        [ObservableProperty]
        double _actualvalueMusicTime = 0;

        [ObservableProperty]
        bool _enabledFinish = true;

        [ObservableProperty]
        TimeSpan _positionVlue = TimeSpan.Zero;

        [ObservableProperty]
        bool _IsFinishPlay = false;

        [ObservableProperty]
        private ObservableCollection<MenuMusicsModel> _menuItems = new()
        {
            new MenuMusicsModel(){
                Name = "Musicas",
                IsActive = true,
                Icon = SymbolRegular.MusicNote124
            },
            new MenuMusicsModel()
            {
                Name = "Albums",
                Icon = SymbolRegular.Album24,

            },
            new MenuMusicsModel(){ 
                Name="Artists",
                Icon = SymbolRegular.Person24
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
                App.Current.Dispatcher.Invoke(() => {
                    Musics?.GroupBy(x => x.Album).ToList().ForEach(x => {
                        Albums.Add(new AlbumModel()
                        {
                            AlbumKey = x.Key,
                            Artist = x.Select(y => y.Artist).Distinct(),
                            Img = x.First().Img
                        });
                    });

                    Musics?.GroupBy(x => x.Artist).ToList().ForEach(x => {
                        Artists.Add(new()
                        {
                            ArtistKey = x.Key,
                        });
                    });

                    Musics?.GroupBy(x => _service.GetKeyForGroup(x)).OrderBy(x => x.Key).ToList().ForEach(x => {
                        GrupoMusic.Add(new GroupMusic() { 
                            Key = x.Key,
                            Musics = x
                        });
                    });
                });
            });
        }

        [RelayCommand]
        void Next() {
            if (MusicSelected != null && Musics != null)
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
            if (MusicSelected != null && Musics != null)
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
        void BuscarMusic(string musica) {
            if (_service == null) return;
            GrupoMusic.Clear();
            Musics?.Where(x => x.Title.ToLower().Contains(musica))
                .GroupBy(_service.GetKeyForGroup)
                .OrderBy(x => x.Key).ToList().ForEach(x =>
                {
                    
                    GrupoMusic.Add(new GroupMusic()
                    {
                        Key = x.Key,
                        Musics = x
                    });
                });
        }

        [RelayCommand]
        void PlaySearchInPlayList(ObservableCollection<GroupMusic> musics) {
            //musics.ToList().ForEach(x => x.Musics.ToList().ForEach(y => Playlist.Add(y)));
            //MusicSelected = null;
            //IsFinishPlay = true;
            //PlayOrPauseCommand.Execute(null);
        }

        [RelayCommand]
        void PlayOrPause()
        {
            if (Mediaelement != null && Musics != null)
            {
                if (Symbolplay != SymbolRegular.Play20)
                {
                    Mediaelement.Pause();
                    Symbolplay = SymbolRegular.Play20;
                }
                else
                {
                    if (MusicSelected == null)
                    {
                        MusicSelected = Musics[0];
                    }

                    if (IsFinishPlay)
                    {
                        var mselect = MusicSelected;
                        MusicSelected = null;
                        MusicSelected = mselect;
                        IsFinishPlay = false;
                    }
                    Mediaelement.Play();
                    Symbolplay = SymbolRegular.Pause20;
                }
            }
        }

    }
}
