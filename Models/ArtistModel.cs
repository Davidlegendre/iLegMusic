namespace iLegMusic.Models;

public class ArtistModel
{
    public string ArtistKey { get; set; }
    public string InitialsName {
        get {
            var splits = ArtistKey.Split(' ');
            return splits.Length > 1 ? splits[0][0].ToString().ToUpper() + splits[1][0].ToString().ToUpper() : splits[0][0].ToString().ToUpper();
        }
    }
}
