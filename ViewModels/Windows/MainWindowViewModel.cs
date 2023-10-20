using iLegMusic.Models;
using iLegMusic.Services;
using iLegMusic.Views.Windows;
using iZathfit.Components.Paginator;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TagLib.Ogg;
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

    private void _paginatorcomponent_ChangePageEvent(object? sender, EventArgs e)
    {
        paginar(MusicsSearch != null ? MusicsSearch : Musics);
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

    public PaginatorComponent? _paginatorcomponent;
    IEnumerable<MusicModel>? MusicsSearch;
 
    [ObservableProperty]
    bool _isIndeterminated = false;

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
    Visibility _FoldersVisible = Visibility.Collapsed;

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
    ObservableCollection<FoldersMusicModel> _folders = new ObservableCollection<FoldersMusicModel>();

    [ObservableProperty]
    ObservableCollection<BreadcrumbBarItem> _breadcrumbbarlist = new ObservableCollection<BreadcrumbBarItem>();

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
        },
        new MenuMusicsModel(){ 
            Name = "Folders",
            Icon = SymbolRegular.Folder20
        }
    };

    public MediaElement? Mediaelement;

    void paginar(IEnumerable<MusicModel>? sourcer = null, bool isNewCollection = false)
    {
        if (Musics != null && _service != null && _paginatorcomponent != null)
        {

            GrupoMusic.Clear();
            var lista = _paginatorcomponent.GetPaginationCollection(sourcer != null ? 
                new ObservableCollection<MusicModel>(sourcer) : Musics, isNewCollection);
            lista.OrderBy(x => x.Title).GroupBy(_service.GetKeyForGroup).ToList().ForEach(x =>
            {
                GrupoMusic.Add(new GroupMusic() { Key = x.Key, Musics = x });
            });
            App.GetService<MainWindow>().Alzeimer();
        }
    }

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
            IsIndeterminated = false;
            if (Musics == null || Musics.Count == 0)
            {
                MessageBox.Show("No hay Musicas en la ruta " + User);
                return;
            }
            VisibleHub = Visibility.Collapsed;
            VisibleMain = Visibility.Visible;
            MusicsVisible = Visibility.Visible;
            App.Current.Dispatcher.Invoke(() => {
                Musics = Musics != null? new ObservableCollection<MusicModel>(Musics.OrderBy(x => x.Title)) : null;
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
                Musics?.GroupBy(x => x.Folder).ToList().ForEach(x => {
                    Folders.Add(new FoldersMusicModel() { UrlOriginal = x.Key, Title = x.Key.Substring(x.Key.LastIndexOf('\\') + 1) });
                });

                if (_paginatorcomponent != null)
                    _paginatorcomponent.ChangePageEvent += _paginatorcomponent_ChangePageEvent;
                paginar();
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
        if (Musics == null || album == null) return;
        MusicsSearch = Musics.Where(m => m.Album == album.AlbumKey);
        paginar(MusicsSearch, true);

    }

    [RelayCommand]
    void DetalleFolder(FoldersMusicModel foldersMusic) { 
        if(Musics == null || foldersMusic == null) return;
        MusicsSearch = Musics.Where(x => x.Folder == foldersMusic.UrlOriginal);
        Breadcrumbbarlist.Clear();
        var folders = foldersMusic.UrlOriginal.Split('\\');
        int i = 0;
        folders.ToList().ForEach(x => {
            i += 1;
            Breadcrumbbarlist.Add(new BreadcrumbBarItem() { Title = x, Id = i });
        }) ;
        Breadcrumbbarlist.Last().LeftSymbolVisibility =  Visibility.Collapsed;
        paginar(MusicsSearch, true);
        
    }

    [RelayCommand]
    void DetalleFolderWithRoute(string ruta)
    {
        if (Musics == null || string.IsNullOrWhiteSpace(ruta)) return;
        MusicsSearch = Musics.Where(x => x.Folder.Contains(ruta));
        paginar(MusicsSearch, true);
    }

    

    [RelayCommand]
    void showLetters(TextBlock textBlock) {
        Musics?.ToList().ForEach(m => m.IsVisible = m.IsVisible == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
        textBlock.BringIntoView();
    }

    [RelayCommand]
    void DetaleArtist(ArtistModel artista)
    {
        if (Musics == null || artista == null) return;
        MusicsSearch = Musics.Where(m => m.Artist == artista.ArtistKey);
        paginar(MusicsSearch, true);
    }

    [RelayCommand]
    void CloseDetalle() {
        MusicsSearch = null;        
        paginar(isNewCollection: true);
    }

    [RelayCommand]
    void Next() {
        if (MusicSelected != null && Musics != null)
        {

            var index = MusicsSearch != null ? 
                MusicsSearch.ToList().Exists(x => x == MusicSelected) ? MusicsSearch.ToList().IndexOf(MusicSelected) : -1
                : Musics.IndexOf(MusicSelected);
            if (index == (MusicsSearch != null ? MusicsSearch.Count() - 1 : Musics.Count - 1) 
                && Iconrepeat == SymbolRegular.ArrowRepeatAll20)
                index = 0;
            else
                index += 1;

            if (index != Musics.Count && MusicsSearch == null)
            {
                MusicSelected = Musics[index];
                Mediaelement?.Play();
            }
            else if(index != MusicsSearch?.Count())
            {
                MusicSelected = MusicsSearch?.ToList()[index];
                Mediaelement?.Play();
            }
        }
    }

    [RelayCommand]
    void Back()
    {
        if (MusicSelected != null && Musics != null)
        {
            var index = MusicsSearch != null ?
               MusicsSearch.ToList().Exists(x => x == MusicSelected) ? MusicsSearch.ToList().IndexOf(MusicSelected) : 
               MusicsSearch.Count()
               : Musics.IndexOf(MusicSelected);

            if (index == 0
               && Iconrepeat == SymbolRegular.ArrowRepeatAll20)
                index = MusicsSearch != null? MusicsSearch.Count() - 1 : Musics.Count - 1;
            else
                index -= 1;

            if (index != -1)
            {
                MusicSelected = MusicsSearch != null ? MusicsSearch.ToList()[index] : Musics[index];
                Mediaelement?.Play();
            }
        }
    }

    [RelayCommand]
    void BuscarMusic(string musica) {
        if (_service == null) return;
        MusicsSearch = Musics?.Where(m => m.Title.ToLower().Contains(musica));
        paginar(MusicsSearch, true);
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
                    MusicSelected = MusicsSearch != null ? MusicsSearch.First() : Musics[0];
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
