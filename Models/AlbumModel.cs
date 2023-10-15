using System.Windows.Media;

namespace iLegMusic.Models;

public class AlbumModel
{
    public string AlbumKey { get; set; }
    public IEnumerable<string> Artist { get; set; }
    public ImageBrush? Img { get; set; }
    public string ArtistsStrings => string.Join(", ", Artist);
}
