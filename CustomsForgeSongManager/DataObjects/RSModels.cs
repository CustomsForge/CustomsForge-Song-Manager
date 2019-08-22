using System.Collections.Generic;
using Newtonsoft.Json;

namespace CustomsForgeSongManager.DataObjects
{
    // SongPack related hsan data
    public class RSDataJsonDictionary<T> : Dictionary<string, Dictionary<string, T>> where T : RSDataAbstractBase { }

    public class RSDataAbstract
    {
        public string AlbumArt { get; set; }
        public string ArrangementName { get; set; }
        public bool DLC { get; set; }
        public int LeaderboardChallengeRating { get; set; }
        public string ManifestUrn { get; set; }
        public int MasterID_RDV { get; set; }
        public string PersistentID { get; set; }
        public virtual string SongKey { get; set; }
    }

    public class RSDataAbstractBase : RSDataAbstract
    {
        [JsonIgnore]
        public string SongSource { get; set; }

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
        public double SongDifficulty { get; set; }
        public float SongLength { get; set; }
        public string SongName { get; set; }
        public string SongNameSort { get; set; }
        public int SongYear { get; set; }
        public Dictionary<string, int> Tuning { get; set; }
    }

    public class RS1DlcData : RSDataAbstractBase
    {
        public dynamic DLCKey { get; set; } // setter not used?
    }

    public class RS1DiscData : RSDataAbstractBase
    {
        public string DLCKey { get; set; } // setter not used?
    }

    public class RS2SongsData : RSDataAbstractBase
    {
    }

    public class RS2VocalsData : RSDataAbstract
    {
        public new string PersistentID { get; set; } // added 'new'
        public string SKU { get; set; }
        public bool Shipping { get; set; }
    }

    public class RSBassData : RSDataAbstract
    {
        public int BassPick { get; set; }
    }

    public class RS1DiscVocalsData : RS2VocalsData
    {
        public string DLCKey { get; set; }
    }

    public class RS1DLCVocalsData : RS2VocalsData
    {
        public dynamic DLCKey { get; set; }
    }

    public class RSLessonSongData : RS2SongsData
    {
        public override string SongKey
        {
            get { return base.SongKey; }
            set
            {
                base.SongKey = value;
                LessonKey = value;
            }
        }

        public string LessonKey { get; set; }
    }

    public class RSUbisoftLessonSongData : RS2SongsData
    {
        public override string SongKey
        {
            get { return base.SongKey; }
            set
            {
                base.SongKey = value;
                EtudeKey = value;
            }
        }

        public string EtudeKey { get; set; }
    }

    public class SongPackData
    {
        public bool Selected { get; set; }
        public string Enabled { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Tuning { get; set; }
        public int SongYear { get; set; }
        public string SongLength { get; set; }
        public string SongKey { get; set; }
    }
}