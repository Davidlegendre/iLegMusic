using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace iLegMusic.Models
{
    public class MenuMusicsModel : INotifyPropertyChanged
    {
        bool _isactive = false;
        public string Name { get; set; }
        public bool IsActive
        {
            get => _isactive; set
            {
                if(value != _isactive)
                {
                    _isactive = value;
                    change(nameof(IsActive));
                    change(nameof(SelectBarVisible));
                    change(nameof(LetraVisible));
                    change(nameof(Opacity));
                }
            }
        }

        public Visibility SelectBarVisible => IsActive ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LetraVisible => IsActive ? Visibility.Visible : Visibility.Collapsed;
        public double Opacity => IsActive ? 1 : 0.5;

        public SymbolRegular Icon { get; set; }


        void change(string property)
        { 
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
