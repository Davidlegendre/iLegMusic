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
    public HomeMusicComponent()
    {
        InitializeComponent();
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
            _vm.IsManipulatingSlider = false;
            _vm.Mediaelement.Position = TimeSpan.FromSeconds(_vm.ActualvalueMusicTime);
            _vm.PositionVlue = TimeSpan.FromSeconds(_vm.ActualvalueMusicTime);
            _vm.ActualvalueMusicTime = slider.Value;
        }
        
    }

    private void Slider_GotMouseCapture(object sender, MouseEventArgs e)
    {
        var _vm = DataContext as MainWindowViewModel;
        if (_vm != null)
        {
            _vm.IsManipulatingSlider = true;
        }
    }

    private void enterinslider_mouseenter(object sender, MouseEventArgs e)
    {
        slider.Visibility = Visibility.Visible;
    }

    private void enterinslider_mouseleave(object sender, MouseEventArgs e)
    {
        slider.Visibility = Visibility.Collapsed;
    }
}
