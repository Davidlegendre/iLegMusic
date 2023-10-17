using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iLegMusic.Models;

public class GroupMusic : INotifyPropertyChanged
{
    Visibility _IsVisible = Visibility.Visible;
    public string Key { get; set; }

    public IEnumerable<MusicModel> Musics { get; set; }

    public Visibility IsVisible
    {
        get => _IsVisible; 
        set
        {
            if(value != _IsVisible)
            {
                _IsVisible = value;
                change(nameof(IsVisible));
            }
        }
    }

    void change(string property)
    {
        if(PropertyChanged!= null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}