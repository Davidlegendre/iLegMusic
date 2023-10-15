using iLegMusic.Models;
using iLegMusic.Services;
using iLegMusic.ViewModels.Windows;
using iLegMusic.Views.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace iLegMusic.Views.Pages;

/// <summary>
/// Lógica de interacción para HomeMusicComponent.xaml
/// </summary>
public partial class HomeMusicComponent : UserControl
{
    LegMusicServiceGlobal _Service;
    public HomeMusicComponent()
    {
        InitializeComponent();
        _Service = App.GetService<LegMusicServiceGlobal>();
        this.SizeChanged += HomeMusicComponent_SizeChanged;
    }

    private void HomeMusicComponent_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var _vm = DataContext as MainWindowViewModel;
        if(_vm != null)
        {
            _vm.Colums = _Service.ColumnsFromWidthWindow(Convert.ToInt32(this.ActualWidth));
        }
    }

    private void openMenuVol_click(object sender, RoutedEventArgs e)
    {
        menuvol.Show();
    }

    private void menuchecked_checked(object sender, RoutedEventArgs e)
    {
        var _vm = DataContext as MainWindowViewModel;
        var radio = (RadioButton)sender;
        var context = radio.DataContext as MenuMusicsModel;
        if (context != null && _vm != null)
        {
            _vm.MusicsVisible = Visibility.Collapsed;
            _vm.AlbumVisible = Visibility.Collapsed;
            _vm.ArtistVisible = Visibility.Collapsed;
            if (context.Icon == SymbolRegular.MusicNote124)
            {
                _vm.MusicsVisible = Visibility.Visible;
                _vm.CloseDetalleCommand.Execute(null);
                return;
            }
            if (context.Icon == SymbolRegular.Album24)
            {
                _vm.AlbumVisible = Visibility.Visible;
                return;
            }
            if (context.Icon == SymbolRegular.Person24)
            {
                _vm.ArtistVisible = Visibility.Visible;
                return;
            }
        }
    }

    private void buscartxt_keydown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var text = sender as Wpf.Ui.Controls.TextBox;
            var _vm = DataContext as MainWindowViewModel;
            if (text != null && _vm != null)
            {
                _vm.BuscarMusicCommand.Execute(text.Text);
            }
        }
    }

    private void Slider_LostMouseCapture(object sender, MouseEventArgs e)
    {
        var _vm = DataContext as MainWindowViewModel;
        var slider = (Slider)sender;
        
        if (_vm != null && _vm.MusicSelected != null && _vm.Mediaelement != null) {         
            _vm.Mediaelement.Position = TimeSpan.FromSeconds(slider.Value);
            _vm.IsManipulatingSlider = false;
        }
        
    }

    private void Slider_GotMouseCapture(object sender, MouseEventArgs e)
    {
        var _vm = DataContext as MainWindowViewModel;
        if(_vm != null)
        _vm.IsManipulatingSlider = true;
    }
}
