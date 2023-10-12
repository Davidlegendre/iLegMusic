using System.Windows.Media;

namespace iLegMusic.Models
{
    public class MusicModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public ImageBrush? Img { get; set; }
        public string url { get; set; }
    }
}
