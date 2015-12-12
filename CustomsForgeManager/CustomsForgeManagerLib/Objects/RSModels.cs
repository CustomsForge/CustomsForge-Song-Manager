using CustomsForgeManager.CustomsForgeManagerLib;
using System.Collections.Generic;
public abstract class RSDataAbstract
{
    public string SongSource { get; set; }
    public string AlbumArt { get; set; }
    public string ArrangementName { get; set; }
    public bool DLC { get; set; }
    public int LeaderboardChallengeRating { get; set; }
    public string ManifestUrn { get; set; }
    public int MasterID_RDV { get; set; }
    public string PersistentID { get; set; }
    public virtual string SongKey { get; set; }
}
public abstract class RSDataAbstractBase : RSDataAbstract
{
    public string AlbumName { get; set; }
    public string AlbumNameSort { get; set; }
    public string ArtistName { get; set; }
    public string ArtistNameSort { get; set; }
    public float CentOffset { get; set; }
    public float DNA_Chords { get; set; }
    public float DNA_Riffs { get; set; }
    public float DNA_Solo { get; set; }
    public float EasyMastery { get; set; }
    public float MediumMastery { get; set; }
    public float NotesEasy { get; set; }
    public float NotesHard { get; set; }
    public float NotesMedium { get; set; }
    public int Representative { get; set; }
    public int RouteMask { get; set; }
    public string SKU { get; set; }
    public bool Shipping { get; set; }
    public float SongDiffEasy { get; set; }
    public float SongDiffHard { get; set; }
    public float SongDiffMed { get; set; }
    public float SongDifficulty { get; set; }
    public float SongLength { get; set; }
    public string SongName { get; set; }
    public string SongNameSort { get; set; }
    public int SongYear { get; set; }
    public Dictionary<string, int> Tuning { get; set; }
}


class RS1DlcData : RSDataAbstractBase
{
    public dynamic DLCKey { get; set; }  // setter not used?
}

class RS1DiscData : RSDataAbstractBase
{
    public string DLCKey { get; set; } // setter not used?
}

class RS2SongsData : RSDataAbstractBase
{

}


class RS2VocalsData : RSDataAbstract
{
    public new string PersistentID { get; set; }  // added 'new'
    public string SKU { get; set; }
    public bool Shipping { get; set; }
}

class RS1DiscVocalsData : RS2VocalsData
{
    public string DLCKey { get; set; }
}

class RS1DLCVocalsData : RS1DiscVocalsData
{
    public new dynamic DLCKey { get; set; } // added 'new'
}
class RSLessonSongData : RS2SongsData
{
    public override string SongKey
    {
        get
        {
            return base.SongKey;
        }
        set
        {
            base.SongKey = value;
            LessonKey = value;
        }
    }
    public string LessonKey { get; set; }
}

class RSUbisoftLessonSongData : RS2SongsData
{
    public override string SongKey
    {
        get
        {
            return base.SongKey;
        }
        set
        {
            base.SongKey = value;
            EtudeKey = value;
        }
    }

    public string EtudeKey { get; set; }
}

class CacheSongData
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string Tuning { get; set; }
    public string SongKey { get; set; }
    public string Enabled { get; set; }
}