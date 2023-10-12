using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Id3Lib;
using iLegMusic.Models;
using Mp3Lib;

namespace iLegMusic.Helpers
{
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
        byte[]? getIMGMP3(Mp3File mp3)
        {
            var tag = mp3.TagModel.Where(x => x.GetType() == typeof(Id3Lib.Frames.FramePicture)).Cast<Id3Lib.Frames.FramePicture>().Where(x => x.FrameId == "APIC");
            var img = tag.FirstOrDefault(x => x.PictureData.Length > 0);
            return img?.PictureData;
        }

        public MusicModel GetMusicModel(string mp3file) {
           
            try
            {
                
                var mp3 = new Mp3File(file: mp3file);
                var Img = obtenerimagen(getIMGMP3(mp3));
                return new MusicModel()
                {
                    Album = !string.IsNullOrWhiteSpace(mp3.TagHandler.Album) ? mp3.TagHandler.Album : "Album Desconocido",
                    Artist = !string.IsNullOrWhiteSpace(mp3.TagHandler.Artist) ? mp3.TagHandler.Artist : "Artista Desconocido",
                    Title = !string.IsNullOrWhiteSpace(mp3.TagHandler.Title) ? mp3.TagHandler.Title :
                     Path.GetFileNameWithoutExtension(mp3file),
                    url = mp3file,
                    Img = Img
                };
            }
            catch {
                return new MusicModel()
                {
                    Album = "Album Desconocido",
                    Artist = "Artista Desconocido",
                    Title = Path.GetFileNameWithoutExtension(mp3file),
                    url= mp3file
                };
            }
        }
    }
}
