using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Media;

namespace iLegMusic.Models
{
    public class MusicModel : INotifyPropertyChanged
    {
        bool _isinplay = false;
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public ImageBrush? Img { get; set; }
        public string url { get; set; }
        public bool IsInPlay
        {
            get => _isinplay; set
            {
                if (_isinplay != value)
                {
                    _isinplay = value;
                    change(nameof(IsInPlay));
                    change(nameof(PlayVisible));
                }
            }
        }

        public Visibility PlayVisible => IsInPlay? Visibility.Visible : Visibility.Collapsed;

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
