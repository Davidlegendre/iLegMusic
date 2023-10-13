using iLegMusic.Models;
using iLegMusic.Services;
using iLegMusic.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace iLegMusic.Views.Pages
{
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

        private void gotodown_click(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
