using iLegMusic.Models;
using iLegMusic.Services;
using iLegMusic.Views.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Common;

namespace iLegMusic.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{

    SymbolRegular[] iconsrepeats = new SymbolRegular[] { SymbolRegular.ArrowRepeatAllOff20,  
        SymbolRegular.ArrowRepeatAll20, SymbolRegular.ArrowRepeat120
    };   

    LegMusicServiceGlobal? _service;
    public MainWindowViewModel()
    {
        _service = App.GetService<LegMusicServiceGlobal>();
        if (_service != null)
            _service.MusicEventFound += _service_MusicEventFound;
    }

    Task? timeTask;
    bool _disposed = false;
    public bool IsManipulatingSlider = false;
    public void InitTask() {
        if (timeTask == null)
        {
            timeTask = new Task(() =>
            {
                while (!_disposed)
                {

                    if (Symbolplay != SymbolRegular.Play20 && Mediaelement != null && IsManipulatingSlider == false)
                    {
                        App.Current.Dispatcher.Invoke(() => {
                            ActualvalueMusicTime = PositionVlue.TotalSeconds;
                            PositionVlue = Mediaelement.Position;
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
        e.ForEach(x => {
            Musics?.Add(x);
        });
        MusicsCount += e.Count();
       
    }

    [ObservableProperty]
    bool _isIndeterminated = false;

    [ObservableProperty]
    int _colums = 4;

    [ObservableProperty]
    double _volumen = 0.5;

    [ObservableProperty]
    Visibility _visibleMain = Visibility.Collapsed;

    [ObservableProperty]
    Visibility _AlbumVisible = Visibility.Collapsed;

    [ObservableProperty]
    Visibility _ArtistVisible = Visibility.Collapsed;

    [ObservableProperty]
    Visibility _MusicsVisible = Visibility.Visible;

    [ObservableProperty]
    Visibility _visibleHub = Visibility.Visible;

    [ObservableProperty]
    int _MusicsCount = 0;

    [ObservableProperty]
    double _valProgressBar = 1;

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
    SymbolRegular _iconrepeat = SymbolRegular.ArrowRepeatAllOff20;

    [ObservableProperty]
    double _maxValueMusicTime = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TimeSpanPosition))]
    double _actualvalueMusicTime = 0;

    [ObservableProperty]
    bool _enabledFinish = true;

    [ObservableProperty]
    TimeSpan _positionVlue = TimeSpan.Zero;

    public TimeSpan TimeSpanPosition => TimeSpan.FromSeconds(ActualvalueMusicTime);

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
        if (!System.IO.Directory.Exists(User))
        {
            System.Windows.MessageBox.Show("La ruta no es correcta o no existe");
            return;
        }
        _service?.GetFilesWithSource(() =>
        {
            VisibleIFFind = Visibility.Visible;
            EnabledFinish = false;
            IsIndeterminated = true;
        }, () =>
        {
            VisibleIFFind = Visibility.Collapsed;
            EnabledFinish = true;
            VisibleHub = Visibility.Collapsed;
            VisibleMain = Visibility.Visible;
            MusicsVisible = Visibility.Visible;
            IsIndeterminated = false;

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
                Musics?.GroupBy(x => _service.GetKeyForGroup(x)).OrderBy(x => x.Key).ToList().ForEach(x =>
                {
                    GrupoMusic.Add(new GroupMusic() { Key = x.Key, Musics = x });
                });
                App.GetService<MainWindow>().Alzeimer();
            });

            
        }, User);
    }

    [RelayCommand]
    void changeSource() {
        if (System.Windows.MessageBox.Show("Desea Buscar en Otra Ruta? esto tardara un poco y cerraremos todo", 
            "Precaucion", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
            return;
        VisibleIFFind = Visibility.Collapsed;
        EnabledFinish = true;
        VisibleHub = Visibility.Visible;
        VisibleMain = Visibility.Collapsed;
        AlbumVisible = Visibility.Collapsed;
        ArtistVisible = Visibility.Collapsed;
        MusicsVisible = Visibility.Collapsed;
        Mediaelement?.Stop();
        Symbolplay = SymbolRegular.Play20;
        MusicSelected = null;
        Musics?.Clear();
        GrupoMusic.Clear();
        Albums.Clear();
        Artists.Clear();
        _disposed = true;
        timeTask = null;
        _disposed = false;
        MusicsCount = 0;
        IsFinishPlay = false;
        App.GetService<MainWindow>().Alzeimer();
    }

    [RelayCommand]
    void DetalleAlbum(AlbumModel album) {
        if (Musics == null) return;
        Musics.ToList().ForEach(m => m.IsVisible = m.Album == album.AlbumKey ? Visibility.Visible : Visibility.Collapsed);
        GrupoMusic.ToList().ForEach(x => x.IsVisible = x.Musics.Count(x => x.IsVisible == Visibility.Collapsed) == x.Musics.Count() ? Visibility.Collapsed : Visibility.Visible);
        
    }

    [RelayCommand]
    void showLetters(TextBlock textBlock) {
        Musics?.ToList().ForEach(m => m.IsVisible = m.IsVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
        textBlock.BringIntoView();
    }

    [RelayCommand]
    void DetaleArtist(ArtistModel artista)
    {
        if (Musics == null) return;
        Musics.ToList().ForEach(m => m.IsVisible = m.Artist == artista.ArtistKey ? Visibility.Visible : Visibility.Collapsed);
        GrupoMusic.ToList().ForEach(x => x.IsVisible = x.Musics.Count(x => x.IsVisible == Visibility.Collapsed) == x.Musics.Count() ? Visibility.Collapsed : Visibility.Visible);
    }

    [RelayCommand]
    void CloseDetalle() {
        if(Musics== null) return;
        Musics.ToList().ForEach(m => m.IsVisible = Visibility.Visible);
        GrupoMusic.ToList().ForEach(x => x.IsVisible = Visibility.Visible);
    }

    [RelayCommand]
    void Next() {
        if (MusicSelected != null && Musics != null)
        {
            var index = Musics.IndexOf(MusicSelected);
            if (index == Musics.Count - 1 && Iconrepeat == SymbolRegular.ArrowRepeatAll20)
                index = 0;

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
        Musics?.ToList().ForEach(m => m.IsVisible = m.Title.ToLower().Contains(musica) ? Visibility.Visible : Visibility.Collapsed);
        GrupoMusic.ToList().ForEach(x => x.IsVisible = x.Musics.Count(x => x.IsVisible == Visibility.Collapsed) == x.Musics.Count() ? Visibility.Collapsed : Visibility.Visible);

    }

    [RelayCommand]
    void repeatActive() {
        //indiceactual = 0
        //aumentar = 1
        //si aumentar es mayor, vuelve a cero
        var indice = iconsrepeats.ToList().IndexOf(Iconrepeat);

        Iconrepeat = iconsrepeats.ElementAt(indice + 1 < iconsrepeats.Length ? indice + 1 : 0);
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
