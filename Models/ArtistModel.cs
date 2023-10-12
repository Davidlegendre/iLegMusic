using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLegMusic.Models
{
    public class ArtistModel
    {
        public string ArtistKey { get; set; }
        public string InitialsName {
            get {
                var splits = ArtistKey.Split(' ');
                return splits.Length > 1 ? splits[0].ToUpper() + splits[1].ToUpper() : splits[0].ToUpper();
            }
        }
        public IEnumerable<AlbumModel> Albums { get; set; }

    }
}
