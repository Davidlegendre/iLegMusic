using iLegMusic.Helpers;
using iLegMusic.Models;
using System.IO;


namespace iLegMusic.Services;

public class LegMusicServiceGlobal
{
    //obtencion de musicas
    public int ColumnsFromWidthWindow(int ActuaWidthWindow)
    {
        return ActuaWidthWindow / 270;
    }
    public void GetFilesWithSource(Action init, Action done, string source = "C:\\Users") {
        new Task(() => {
            init();
            GetFiles(source);
            done();
        }).Start();
    }

    public string GetKeyForGroup(MusicModel x) {
        var firstcaracter = x.Title.ToUpper().Trim()[0];
        return char.IsNumber(firstcaracter) ? "#" : firstcaracter.ToString();
           
    }

    void GetFiles(string source = "C:/")
    {
        string[]? files = null;
        try { files = Directory.GetFiles(source); } catch { };
        string[]? foldres = null;
        try { foldres = Directory.GetDirectories(source); } catch { };
        List<MusicModel>? musics = null;
        files?.Where(x => x.EndsWith(".mp3")).ToList().ForEach(f => {
            if (musics == null) musics = new List<MusicModel>();

            App.Current.Dispatcher.Invoke(() =>
            {
                var data = new HelperMusicRender().GetMusicModel(f);
                
                if (data != null)                        
                {
                    data.Folder = source;
                    musics.Add(data);                        
                }
            });
        });

        if (musics != null)
        {                

            App.Current.Dispatcher.Invoke(() => {
                if (MusicEventFound != null)
                {
                    MusicEventFound.Invoke(this, musics);
                }
            });
        }           

        foldres?.ToList().ForEach(f => {
            GetFiles(f);
        });
    }

    public event EventHandler<List<MusicModel>>? MusicEventFound;
}
