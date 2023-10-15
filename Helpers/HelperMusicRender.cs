using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using iLegMusic.Models;
namespace iLegMusic.Helpers;

public class HelperMusicRender
{
    ImageBrush? obtenerimagen(byte[]? bys, int Pixel = 256)
    {
        if (bys is null) return null;
        ImageBrush? img;
        using (var ms = new MemoryStream(bys))
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.DecodePixelWidth = Pixel;
            bitmapImage.EndInit();
            img = new ImageBrush();
            img.ImageSource = bitmapImage;
            img.AlignmentX = AlignmentX.Center;
            img.AlignmentY = AlignmentY.Top;
            img.Stretch = Stretch.UniformToFill;
            
        }
        GC.Collect();
        return img;
    }

    public MusicModel? GetMusicModel(string mp3file) {

        try
        {
            var taglibfile = TagLib.File.Create(mp3file);
            try
            {

                var imgs = taglibfile.Tag.Pictures != null && taglibfile.Tag.Pictures.Length > 0 ? taglibfile.Tag.Pictures[0] : null;
                var Img = obtenerimagen(imgs?.Data.Data);
                return new MusicModel()
                {
                    Album = !string.IsNullOrWhiteSpace(taglibfile.Tag.Album) ? taglibfile.Tag.Album : "Album Desconocido",
                    Artist = taglibfile.Tag.AlbumArtists != null && taglibfile.Tag.AlbumArtists.Length > 0 ? string.Join(", ", taglibfile.Tag.AlbumArtists) : "Artista Desconocido",
                    Title = !string.IsNullOrWhiteSpace(taglibfile.Tag.Title) ? taglibfile.Tag.Title :
                     Path.GetFileNameWithoutExtension(mp3file),
                    url = mp3file,
                    Img = Img,
                    Duration = taglibfile.Properties.Duration.Hours.ToString("00") + ":" +
                    taglibfile.Properties.Duration.Minutes.ToString("00") + ":" +
                    taglibfile.Properties.Duration.Seconds.ToString("00"),
                };
            }
            catch
            {
                return new MusicModel()
                {
                    Album = "Album Desconocido",
                    Artist = "Artista Desconocido",
                    Title = Path.GetFileNameWithoutExtension(mp3file),
                    url = mp3file,
                    Duration = taglibfile.Properties.Duration.Hours.ToString("00") + ":" +
                    taglibfile.Properties.Duration.Minutes.ToString("00") + ":" +
                    taglibfile.Properties.Duration.Seconds.ToString("00"),
                };
            }
        }
        catch { return null; }
    }
}
