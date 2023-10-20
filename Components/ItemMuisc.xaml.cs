using iLegMusic.Models;
using iLegMusic.ViewModels.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace iLegMusic.Components
{
    /// <summary>
    /// Lógica de interacción para ItemMuisc.xaml
    /// </summary>
    public partial class ItemMuisc : UserControl
    {
        public ItemMuisc()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var txtblock = (TextBlock)sender;
            var _vm = Tag as MainWindowViewModel;
            if(_vm != null && txtblock != null)
            {
                _vm.showLettersCommand.Execute(txtblock);
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var _vm = Tag as MainWindowViewModel;
            if (_vm != null)
            {
                var btn = (Grid)sender;
                var context = btn.DataContext as MusicModel;
                if (context != null)
                {
                    _vm.MusicSelected = context;
                    _vm.Symbolplay = SymbolRegular.Play20;
                    _vm.PlayOrPauseCommand.Execute(null);
                }
            }
        }
    }
}
