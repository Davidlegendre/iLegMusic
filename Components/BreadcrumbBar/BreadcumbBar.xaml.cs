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

namespace iLegMusic.Components.BreadcrumbBar
{
    /// <summary>
    /// Lógica de interacción para BreadcumbBar.xaml
    /// </summary>
    public partial class BreadcumbBar : UserControl
    {
        public BreadcumbBar()
        {
            InitializeComponent();
        }


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var _vm = DataContext as MainWindowViewModel;
            if (_vm != null)
            {
                lista.SelectedItem = null;
                _vm.Breadcrumbbarlist.Clear();
               
            }
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _vm = DataContext as MainWindowViewModel;
            var context = lista.SelectedItem as BreadcrumbBarItem;
            if (_vm != null && context != null)
            {

                var url = string.Join("\\", _vm.Breadcrumbbarlist.Where(x => x.Id <= context.Id).Select(x => x.Title));
                _vm.DetalleFolderWithRouteCommand.Execute(url);

            }
        }
    }
}
