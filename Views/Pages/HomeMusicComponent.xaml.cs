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
    /// Lógica de interacción para HomeMusicComponent.xaml
    /// </summary>
    public partial class HomeMusicComponent : UserControl
    {
        public HomeMusicComponent()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var _vm = DataContext as MainWindowViewModel;
            if (_vm != null)
            {
                var btn = (Wpf.Ui.Controls.CardAction)sender;
                var context = btn.DataContext as MusicModel;
                if (context != null)
                {
                    _vm.MusicSelected = context;
                    _vm.Mediaelement?.Play();
                }
            }
        }
    }
}
