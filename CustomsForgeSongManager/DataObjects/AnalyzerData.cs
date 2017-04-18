using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomsForgeSongManager.DataObjects
{
    class AnalyzerData
    {
        private List<ArrangementData> _arrangements = new List<ArrangementData>();

        public List<ArrangementData> Arrangements
        {
            get { return _arrangements; }
            set { _arrangements = value; }
        }

        public string Artist { get; set; }
        public string SongName { get; set; }
    }

    class ArrangementData  
    {
        private int _octaves = 0, _pullOffs = 0, _bends = 0, _hammerOns = 0, _harmonics = 0, _mutes = 0, _palmMutes = 0, _plucks = 0, _slaps = 0, _sustains = 0, _pops = 0, _slides = 0, _tremolos = 0, _harmonicPinches = 0, _unpitchedSlides = 0, _taps = 0, _vibratos = 0;

        public string ArrangementName { get; set; }
        public Dictionary<string, int> Chords { get; set; }
        public int Octaves { get { return _octaves; } set { _octaves = value; } }
        public int Bends { get { return _bends; } set { _bends = value; } }
        public int HammerOns { get { return _hammerOns; } set { _hammerOns = value; } }
        public int PullOffs { get { return _pullOffs; } set { _pullOffs = value; } }
        public int Harmonics { get { return _harmonics; } set { _harmonics = value; } }
        public int FretHandMutes { get { return _mutes; } set { _mutes = value; } }
        public int PalmMutes { get { return _palmMutes; } set { _palmMutes = value; } }
        public int Plucks { get { return _plucks; } set { _plucks = value; } }
        public int Slaps { get { return _slaps; } set { _slaps = value; } }
        public int Pops { get { return _pops; } set { _pops = value; } }
        public int Slides { get { return _slides; } set { _slides = value; } }
        public int Sustains { get { return _sustains; } set { _sustains = value; } }
        public int Tremolos { get { return _tremolos; } set { _tremolos = value; } }
        public int HarmonicPinches { get { return _harmonicPinches; } set { _harmonicPinches = value; } }
        public int UnpitchedSlides { get { return _unpitchedSlides; } set { _unpitchedSlides = value; } }
        public int Taps { get { return _taps; } set { _taps = value; } }
        public int Vibratos { get { return _vibratos; } set { _vibratos = value; } }
    }
}
