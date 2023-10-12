using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLegMusic.Models
{
    public class GroupMusic
    {
        public string Key { get; set; }
        public IEnumerable<MusicModel> Musics { get; set;}
    }
}
