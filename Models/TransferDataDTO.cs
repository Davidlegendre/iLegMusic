using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLegMusic.Models
{
    public class TransferDataDTO
    {
        public IEnumerable<MusicModel> Music { get; set; }
        public IEnumerable<AlbumModel> Album { get; set; }
        public IEnumerable<ArtistModel> Artist { get; set; }
    }
}
