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

namespace iLegMusic.Views.Pages
{
    /// <summary>
    /// Lógica de interacción para MusicPage.xaml
    /// </summary>
    public partial class MusicPage : UserControl
    {
        LegMusicServiceGlobal _service;
        public MusicPage()
        {
            InitializeComponent();
            _service = App.GetService<LegMusicServiceGlobal>();
            this.Loaded += MusicPage_Loaded;
        }

        

        private void MusicPage_Loaded(object sender, RoutedEventArgs e)
        {
            var _vm = lista.DataContext as MainWindowViewModel;
            if (_vm != null)
            {
                _vm._paginatorcomponent = paginator;
            }
        }

    }
}
