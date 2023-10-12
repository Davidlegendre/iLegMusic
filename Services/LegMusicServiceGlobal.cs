using iLegMusic.Helpers;
using iLegMusic.Models;
using Mp3Lib;
using System.IO;

namespace iLegMusic.Services
{
    public class LegMusicServiceGlobal
    {
        //obtencion de musicas
        public void GetFilesWithSource(Action init, Action done, string source = "C:\\Users") {
            new Task(() => {
                init();
                GetFiles(source);
                done();
            }).Start();
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
                var data = new HelperMusicRender().GetMusicModel(f);
                musics.Add(data);
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
}
