using iLegMusic.Models;
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

namespace iLegMusic.Views.Pages
{
    /// <summary>
    /// Lógica de interacción para AlbumsPages.xaml
    /// </summary>
    public partial class AlbumsPages : UserControl
    {
        public AlbumsPages()
        {
            InitializeComponent();
        }

        private void listAlbums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _vm = DataContext as MainWindowViewModel;
            var item = listAlbums.SelectedItem as AlbumModel;
            if (_vm != null)
            {
                _vm.DetalleAlbumCommand.Execute(item);
            }
        }
    }
}
