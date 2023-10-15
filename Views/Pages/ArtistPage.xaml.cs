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
    /// Lógica de interacción para ArtistPage.xaml
    /// </summary>
    public partial class ArtistPage : UserControl
    {
        
        public ArtistPage()
        {
            InitializeComponent();
        }


        private void listartist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _vm = DataContext as MainWindowViewModel;
            var item = listartist.SelectedItem as ArtistModel;
            if (_vm != null)
            {
                _vm.DetaleArtistCommand.Execute(item);
            }
        }
    }
}
