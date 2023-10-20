using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLegMusic.Models
{
    public class BreadcrumbBarItem
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public Visibility LeftSymbolVisibility { get; set; } = Visibility.Visible;
    }
}
