#if BASS_LIB
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CFSM.AudioTools
{
    public class Bass
    {
        #region Init

        public static bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win)
        {
            return Bass.BASS_Init(device, freq, flags, win, IntPtr.Zero);
        }

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, IntPtr A_4);

        #endregion

        #region Loading

        [DllImport("bass.dll", EntryPoint = "BASS_StreamCreateFile", CharSet = CharSet.Auto)]
        private static extern int BASS_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool A_0, [MarshalAs(UnmanagedType.LPWStr), In] string A_1, long A_2, long A_3, BASSFlag A_4);

        public static int BASS_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
        {
            flags |= BASSFlag.BASS_UNICODE;
            return Bass.BASS_StreamCreateFileUnicode(false, file, offset, length, flags);
        }

        [DllImport("bass.dll", EntryPoint = "BASS_StreamCreateFile")]
        private static extern int BASS_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool inMemory, IntPtr memory, long offset, long length, BASSFlag flags);

        public static int BASS_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
        {
            return Bass.BASS_StreamCreateFileMemory(true, memory, offset, length, flags);
        }

        #endregion

        #region Channel

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelGetAttribute(int handle, BASSAttribute attrib, ref float value);

        [DllImport("bass.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelSetAttribute(int handle, BASSAttribute attrib, float value);

        [DllImport("bass.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelPlay(int handle, [MarshalAs(UnmanagedType.Bool)] bool restart);

        [DllImport("bass.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelStop(int handle);

        [DllImport("bass.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelPause(int handle);

        [DllImport("bass.dll")]
        public static extern long BASS_ChannelSeconds2Bytes(int handle, double pos);

        [DllImport("bass.dll")]
        public static extern double BASS_ChannelBytes2Seconds(int handle, long pos);

        [DllImport("bass.dll")]
        public static extern BASSActive BASS_ChannelIsActive(int handle);

        [DllImport("bass.dll")]
        public static extern long BASS_ChannelGetLength(int handle, BASSMode mode);

        public static long BASS_ChannelGetLength(int handle)
        {
            return Bass.BASS_ChannelGetLength(handle, BASSMode.BASS_POS_BYTES);
        }

        #endregion

        #region Position

        [DllImport("bass.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelSetPosition(int handle, long pos, BASSMode mode);

        public static bool BASS_ChannelSetPosition(int handle, long pos)
        {
            return Bass.BASS_ChannelSetPosition(handle, pos, BASSMode.BASS_POS_BYTES);
        }

        public static bool BASS_ChannelSetPosition(int handle, double seconds)
        {
            return Bass.BASS_ChannelSetPosition(handle, Bass.BASS_ChannelSeconds2Bytes(handle, seconds), BASSMode.BASS_POS_BYTES);
        }

        public static bool BASS_ChannelSetPosition(int handle, int order, int row)
        {
            return Bass.BASS_ChannelSetPosition(handle, (long) Utils.MakeLong(order, row), BASSMode.BASS_POS_MUSIC_ORDERS);
        }

        [DllImport("bass.dll")]
        public static extern long BASS_ChannelGetPosition(int handle, BASSMode mode);

        public static long BASS_ChannelGetPosition(int handle)
        {
            return Bass.BASS_ChannelGetPosition(handle, BASSMode.BASS_POS_BYTES);
        }

        #endregion

        [DllImport("bass.dll")]
        public static extern BASSError BASS_ErrorGetCode();

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_GetInfo([In, Out] BASS_INFO info);

        public static BASS_INFO BASS_GetInfo()
        {
            BASS_INFO info = new BASS_INFO();
            if (Bass.BASS_GetInfo(info))
                return info;
            return (BASS_INFO) null;
        }

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_StreamFree(int handle);

        [DllImport("bass.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_Free();
    }

    [Flags]
    public enum BASSInfo
    {
        DSCAPS_NONE = 0,
        DSCAPS_CONTINUOUSRATE = 16,
        DSCAPS_EMULDRIVER = 32,
        DSCAPS_CERTIFIED = 64,
        DSCAPS_SECONDARYMONO = 256,
        DSCAPS_SECONDARYSTEREO = 512,
        DSCAPS_SECONDARY8BIT = 1024,
        DSCAPS_SECONDARY16BIT = 2048,
    }

    public enum BASSError
    {
        BASS_ERROR_UNKNOWN = -1,
        BASS_OK = 0,
        BASS_ERROR_MEM = 1,
        BASS_ERROR_FILEOPEN = 2,
        BASS_ERROR_DRIVER = 3,
        BASS_ERROR_BUFLOST = 4,
        BASS_ERROR_HANDLE = 5,
        BASS_ERROR_FORMAT = 6,
        BASS_ERROR_POSITION = 7,
        BASS_ERROR_INIT = 8,
        BASS_ERROR_START = 9,
        BASS_ERROR_NOCD = 12,
        BASS_ERROR_CDTRACK = 13,
        BASS_ERROR_ALREADY = 14,
        BASS_ERROR_NOPAUSE = 16,
        BASS_ERROR_NOTAUDIO = 17,
        BASS_ERROR_NOCHAN = 18,
        BASS_ERROR_ILLTYPE = 19,
        BASS_ERROR_ILLPARAM = 20,
        BASS_ERROR_NO3D = 21,
        BASS_ERROR_NOEAX = 22,
        BASS_ERROR_DEVICE = 23,
        BASS_ERROR_NOPLAY = 24,
        BASS_ERROR_FREQ = 25,
        BASS_ERROR_NOTFILE = 27,
        BASS_ERROR_NOHW = 29,
        BASS_ERROR_EMPTY = 31,
        BASS_ERROR_NONET = 32,
        BASS_ERROR_CREATE = 33,
        BASS_ERROR_NOFX = 34,
        BASS_ERROR_PLAYING = 35,
        BASS_ERROR_NOTAVAIL = 37,
        BASS_ERROR_DECODE = 38,
        BASS_ERROR_DX = 39,
        BASS_ERROR_TIMEOUT = 40,
        BASS_ERROR_FILEFORM = 41,
        BASS_ERROR_SPEAKER = 42,
        BASS_ERROR_VERSION = 43,
        BASS_ERROR_CODEC = 44,
        BASS_ERROR_ENDED = 45,
        BASS_ERROR_BUSY = 46,
        BASS_ERROR_VIDEO_ABORT = 47,
        BASS_ERROR_VIDEO_CANNOT_CONNECT = 48,
        BASS_ERROR_VIDEO_CANNOT_READ = 49,
        BASS_ERROR_VIDEO_CANNOT_WRITE = 50,
        BASS_ERROR_VIDEO_FAILURE = 51,
        BASS_ERROR_VIDEO_FILTER = 52,
        BASS_ERROR_VIDEO_INVALID_CHAN = 53,
        BASS_ERROR_VIDEO_INVALID_DLL = 54,
        BASS_ERROR_VIDEO_INVALID_FORMAT = 55,
        BASS_ERROR_VIDEO_INVALID_HANDLE = 56,
        BASS_ERROR_VIDEO_INVALID_PARAMETER = 57,
        BASS_ERROR_VIDEO_NO_AUDIO = 58,
        BASS_ERROR_VIDEO_NO_EFFECT = 59,
        BASS_ERROR_VIDEO_NO_INTERFACE = 60,
        BASS_ERROR_VIDEO_NO_RENDERER = 61,
        BASS_ERROR_VIDEO_NO_SUPPORT = 62,
        BASS_ERROR_VIDEO_NO_VIDEO = 63,
        BASS_ERROR_VIDEO_NOT_ALLOWED = 64,
        BASS_ERROR_VIDEO_NOT_CONNECTED = 65,
        BASS_ERROR_VIDEO_NOT_EXISTS = 66,
        BASS_ERROR_VIDEO_NOT_FOUND = 67,
        BASS_ERROR_VIDEO_NOT_READY = 68,
        BASS_ERROR_VIDEO_NULL_DEVICE = 69,
        BASS_ERROR_VIDEO_OPEN = 70,
        BASS_ERROR_VIDEO_OUTOFMEMORY = 71,
        BASS_ERROR_VIDEO_PARTIAL_RENDER = 72,
        BASS_ERROR_VIDEO_TIME_OUT = 73,
        BASS_ERROR_VIDEO_UNKNOWN_FILE_TYPE = 74,
        BASS_ERROR_VIDEO_UNSUPPORT_STREAM = 75,
        BASS_ERROR_VIDEO_VIDEO_FILTER = 76,
        BASS_ERROR_WMA_LICENSE = 1000,
        BASS_ERROR_WMA_WM9 = 1001,
        BASS_ERROR_WMA_DENIED = 1002,
        BASS_ERROR_WMA_CODEC = 1003,
        BASS_ERROR_WMA_INDIVIDUAL = 1004,
        BASS_ERROR_ACM_CANCEL = 2000,
        BASS_ERROR_CAST_DENIED = 2100,
        BASS_VST_ERROR_NOINPUTS = 3000,
        BASS_VST_ERROR_NOOUTPUTS = 3001,
        BASS_VST_ERROR_NOREALTIME = 3002,
        BASS_FX_ERROR_NODECODE = 4000,
        BASS_FX_ERROR_BPMINUSE = 4001,
        BASS_ERROR_WASAPI = 5000,
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public sealed class BASS_INFO
    {
        public int minbuf = 500;
        public BASSInfo flags;
        public int hwsize;
        public int hwfree;
        public int freesam;
        public int free3d;
        public int minrate;
        public int maxrate;
        public bool eax;
        public int dsver;
        public int latency;
        public BASSInit initflags;
        public int speakers;
        public int freq;

        public bool SupportsContinuousRate
        {
            get { return (this.flags & BASSInfo.DSCAPS_CONTINUOUSRATE) != BASSInfo.DSCAPS_NONE; }
        }

        public bool SupportsDirectSound
        {
            get { return (this.flags & BASSInfo.DSCAPS_EMULDRIVER) == BASSInfo.DSCAPS_NONE; }
        }

        public bool IsCertified
        {
            get { return (this.flags & BASSInfo.DSCAPS_CERTIFIED) != BASSInfo.DSCAPS_NONE; }
        }

        public bool SupportsMonoSamples
        {
            get { return (this.flags & BASSInfo.DSCAPS_SECONDARYMONO) != BASSInfo.DSCAPS_NONE; }
        }

        public bool SupportsStereoSamples
        {
            get { return (this.flags & BASSInfo.DSCAPS_SECONDARYSTEREO) != BASSInfo.DSCAPS_NONE; }
        }

        public bool Supports8BitSamples
        {
            get { return (this.flags & BASSInfo.DSCAPS_SECONDARY8BIT) != BASSInfo.DSCAPS_NONE; }
        }

        public bool Supports16BitSamples
        {
            get { return (this.flags & BASSInfo.DSCAPS_SECONDARY16BIT) != BASSInfo.DSCAPS_NONE; }
        }

        public override string ToString()
        {
            return string.Format("Speakers={0}, MinRate={1}, MaxRate={2}, DX={3}, EAX={4}", this.speakers, this.minrate, this.maxrate, this.dsver, (this.eax ? true : false));
        }
    }

    public class Utils
    {
        private static int a(IntPtr A_0)
        {
            int ofs = 0;
            while ((int) Marshal.ReadByte(A_0, ofs) != 0)
                ++ofs;
            return ofs;
        }

        public static string IntPtrAsStringUtf8(IntPtr utf8Ptr, out int len)
        {
            len = 0;
            if (utf8Ptr != IntPtr.Zero)
            {
                len = Utils.a(utf8Ptr);
                if (len != 0)
                {
                    byte[] numArray = new byte[len];
                    Marshal.Copy(utf8Ptr, numArray, 0, len);
                    return Encoding.UTF8.GetString(numArray, 0, len);
                }
            }
            return null;
        }

        public static int MakeLong(int lowWord, int highWord)
        {
            return highWord << 16 | lowWord & (int) ushort.MaxValue;
        }

        public static int DBToLevel(double dB, int maxLevel)
        {
            return (int) Math.Round((double) maxLevel*Math.Pow(10.0, dB/20.0));
        }
    }

    #region Enums

    public enum BASSActive
    {
        BASS_ACTIVE_STOPPED,
        BASS_ACTIVE_PLAYING,
        BASS_ACTIVE_STALLED,
        BASS_ACTIVE_PAUSED,
    }

    [Flags]
    public enum BASSMode
    {
        BASS_POS_BYTES = 0,
        BASS_POS_MUSIC_ORDERS = 1,
        BASS_POS_MIDI_TICK = 2,
        BASS_MUSIC_POSRESET = 32768,
        BASS_MUSIC_POSRESETEX = 4194304,
        BASS_MIXER_NORAMPIN = 8388608,
        BASS_POS_DECODE = 268435456,
        BASS_POS_DECODETO = 536870912,
        BASS_MIDI_DECAYSEEK = 16384,
    }

    [Flags]
    public enum BASSSync
    {
        BASS_SYNC_POS = 0,
        BASS_SYNC_MUSICINST = 1,
        BASS_SYNC_END = 2,
        BASS_SYNC_MUSICFX = BASS_SYNC_END | BASS_SYNC_MUSICINST,
        BASS_SYNC_META = 4,
        BASS_SYNC_SLIDE = BASS_SYNC_META | BASS_SYNC_MUSICINST,
        BASS_SYNC_STALL = BASS_SYNC_META | BASS_SYNC_END,
        BASS_SYNC_DOWNLOAD = BASS_SYNC_STALL | BASS_SYNC_MUSICINST,
        BASS_SYNC_FREE = 8,
        BASS_SYNC_MUSICPOS = BASS_SYNC_FREE | BASS_SYNC_END,
        BASS_SYNC_SETPOS = BASS_SYNC_MUSICPOS | BASS_SYNC_MUSICINST,
        BASS_SYNC_OGG_CHANGE = BASS_SYNC_FREE | BASS_SYNC_META,
        BASS_SYNC_MIXTIME = 1073741824,
        BASS_SYNC_ONETIME = -2147483648,
        BASS_SYNC_MIXER_ENVELOPE = 66048,
        BASS_SYNC_MIXER_ENVELOPE_NODE = BASS_SYNC_MIXER_ENVELOPE | BASS_SYNC_MUSICINST,
        BASS_SYNC_WMA_CHANGE = 65792,
        BASS_SYNC_WMA_META = BASS_SYNC_WMA_CHANGE | BASS_SYNC_MUSICINST,
        BASS_SYNC_CD_ERROR = 1000,
        BASS_SYNC_CD_SPEED = BASS_SYNC_CD_ERROR | BASS_SYNC_END,
        BASS_WINAMP_SYNC_BITRATE = 100,
        BASS_SYNC_MIDI_MARKER = 65536,
        BASS_SYNC_MIDI_CUE = BASS_SYNC_MIDI_MARKER | BASS_SYNC_MUSICINST,
        BASS_SYNC_MIDI_LYRIC = BASS_SYNC_MIDI_MARKER | BASS_SYNC_END,
        BASS_SYNC_MIDI_TEXT = BASS_SYNC_MIDI_LYRIC | BASS_SYNC_MUSICINST,
        BASS_SYNC_MIDI_EVENT = BASS_SYNC_MIDI_MARKER | BASS_SYNC_META,
        BASS_SYNC_MIDI_TICK = BASS_SYNC_MIDI_EVENT | BASS_SYNC_MUSICINST,
        BASS_SYNC_MIDI_TIMESIG = BASS_SYNC_MIDI_EVENT | BASS_SYNC_END,
        BASS_SYNC_MIDI_KEYSIG = BASS_SYNC_MIDI_TIMESIG | BASS_SYNC_MUSICINST,
    }

    [Flags]
    public enum BASSInit
    {
        BASS_DEVICE_DEFAULT = 0,
        BASS_DEVICE_8BITS = 1,
        BASS_DEVICE_MONO = 2,
        BASS_DEVICE_3D = 4,
        BASS_DEVICE_LATENCY = 256,
        BASS_DEVICE_CPSPEAKERS = 1024,
        BASS_DEVICE_SPEAKERS = 2048,
        BASS_DEVICE_NOSPEAKER = 4096,
        BASS_DEVIDE_DMIX = 8192,
        BASS_DEVICE_FREQ = 16384,
    }

    public enum BASSAttribute
    {
        BASS_ATTRIB_FREQ = 1,
        BASS_ATTRIB_VOL = 2,
        BASS_ATTRIB_PAN = 3,
        BASS_ATTRIB_EAXMIX = 4,
        BASS_ATTRIB_NOBUFFER = 5,
        BASS_ATTRIB_CPU = 7,
        BASS_ATTRIB_SRC = 8,
        BASS_ATTRIB_MUSIC_AMPLIFY = 256,
        BASS_ATTRIB_MUSIC_PANSEP = 257,
        BASS_ATTRIB_MUSIC_PSCALER = 258,
        BASS_ATTRIB_MUSIC_BPM = 259,
        BASS_ATTRIB_MUSIC_SPEED = 260,
        BASS_ATTRIB_MUSIC_VOL_GLOBAL = 261,
        BASS_ATTRIB_MUSIC_VOL_CHAN = 512,
        BASS_ATTRIB_MUSIC_VOL_INST = 768,
        BASS_ATTRIB_TEMPO = 65536,
        BASS_ATTRIB_TEMPO_PITCH = 65537,
        BASS_ATTRIB_TEMPO_FREQ = 65538,
        BASS_ATTRIB_TEMPO_OPTION_USE_AA_FILTER = 65552,
        BASS_ATTRIB_TEMPO_OPTION_AA_FILTER_LENGTH = 65553,
        BASS_ATTRIB_TEMPO_OPTION_USE_QUICKALGO = 65554,
        BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS = 65555,
        BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS = 65556,
        BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS = 65557,
        BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK = 65558,
        BASS_ATTRIB_REVERSE_DIR = 69632,
        BASS_ATTRIB_MIDI_PPQN = 73728,
        BASS_ATTRIB_MIDI_CPU = 73729,
        BASS_ATTRIB_MIDI_CHANS = 73730,
        BASS_ATTRIB_MIDI_VOICES = 73731,
        BASS_ATTRIB_MIDI_VOICES_ACTIVE = 73732,
        BASS_ATTRIB_MIDI_TRACK_VOL = 73984,
        BASS_ATTRIB_OPUS_ORIGFREQ = 77824,
    }

    public enum BASSFXType
    {
        BASS_FX_DX8_CHORUS = 0,
        BASS_FX_DX8_COMPRESSOR = 1,
        BASS_FX_DX8_DISTORTION = 2,
        BASS_FX_DX8_ECHO = 3,
        BASS_FX_DX8_FLANGER = 4,
        BASS_FX_DX8_GARGLE = 5,
        BASS_FX_DX8_I3DL2REVERB = 6,
        BASS_FX_DX8_PARAMEQ = 7,
        BASS_FX_DX8_REVERB = 8,
        BASS_FX_BFX_ROTATE = 65536,
        BASS_FX_BFX_ECHO = 65537,
        BASS_FX_BFX_FLANGER = 65538,
        BASS_FX_BFX_VOLUME = 65539,
        BASS_FX_BFX_PEAKEQ = 65540,
        BASS_FX_BFX_REVERB = 65541,
        BASS_FX_BFX_LPF = 65542,
        BASS_FX_BFX_MIX = 65543,
        BASS_FX_BFX_DAMP = 65544,
        BASS_FX_BFX_AUTOWAH = 65545,
        BASS_FX_BFX_ECHO2 = 65546,
        BASS_FX_BFX_PHASER = 65547,
        BASS_FX_BFX_ECHO3 = 65548,
        BASS_FX_BFX_CHORUS = 65549,
        BASS_FX_BFX_APF = 65550,
        BASS_FX_BFX_COMPRESSOR = 65551,
        BASS_FX_BFX_DISTORTION = 65552,
        BASS_FX_BFX_COMPRESSOR2 = 65553,
        BASS_FX_BFX_VOLUME_ENV = 65554,
        BASS_FX_BFX_BQF = 65555,
    }

    [Flags]
    public enum BASSFlag
    {
        BASS_DEFAULT = 0,
        BASS_SAMPLE_8BITS = 1,
        BASS_SAMPLE_MONO = 2,
        BASS_SAMPLE_LOOP = 4,
        BASS_SAMPLE_3D = 8,
        BASS_SAMPLE_SOFTWARE = 16,
        BASS_SAMPLE_MUTEMAX = 32,
        BASS_SAMPLE_VAM = 64,
        BASS_SAMPLE_FX = 128,
        BASS_SAMPLE_FLOAT = 256,
        BASS_RECORD_PAUSE = 32768,
        BASS_STREAM_PRESCAN = 131072,
        BASS_STREAM_AUTOFREE = 262144,
        BASS_STREAM_RESTRATE = 524288,
        BASS_STREAM_BLOCK = 1048576,
        BASS_STREAM_DECODE = 2097152,
        BASS_STREAM_STATUS = 8388608,
        BASS_SPEAKER_FRONT = 16777216,
        BASS_SPEAKER_REAR = 33554432,
        BASS_SPEAKER_CENLFE = BASS_SPEAKER_REAR | BASS_SPEAKER_FRONT,
        BASS_SPEAKER_REAR2 = 67108864,
        BASS_SPEAKER_LEFT = 268435456,
        BASS_SPEAKER_RIGHT = 536870912,
        BASS_SPEAKER_FRONTLEFT = BASS_SPEAKER_LEFT | BASS_SPEAKER_FRONT,
        BASS_SPEAKER_FRONTRIGHT = BASS_SPEAKER_RIGHT | BASS_SPEAKER_FRONT,
        BASS_SPEAKER_REARLEFT = BASS_SPEAKER_LEFT | BASS_SPEAKER_REAR,
        BASS_SPEAKER_REARRIGHT = BASS_SPEAKER_RIGHT | BASS_SPEAKER_REAR,
        BASS_SPEAKER_CENTER = BASS_SPEAKER_REARLEFT | BASS_SPEAKER_FRONT,
        BASS_SPEAKER_LFE = BASS_SPEAKER_REARRIGHT | BASS_SPEAKER_FRONT,
        BASS_SPEAKER_REAR2LEFT = BASS_SPEAKER_LEFT | BASS_SPEAKER_REAR2,
        BASS_SPEAKER_REAR2RIGHT = BASS_SPEAKER_RIGHT | BASS_SPEAKER_REAR2,
        BASS_SPEAKER_PAIR1 = BASS_SPEAKER_FRONT,
        BASS_SPEAKER_PAIR2 = BASS_SPEAKER_REAR,
        BASS_SPEAKER_PAIR3 = BASS_SPEAKER_PAIR2 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR4 = BASS_SPEAKER_REAR2,
        BASS_SPEAKER_PAIR5 = BASS_SPEAKER_PAIR4 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR6 = BASS_SPEAKER_PAIR4 | BASS_SPEAKER_PAIR2,
        BASS_SPEAKER_PAIR7 = BASS_SPEAKER_PAIR6 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR8 = 134217728,
        BASS_SPEAKER_PAIR9 = BASS_SPEAKER_PAIR8 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR10 = BASS_SPEAKER_PAIR8 | BASS_SPEAKER_PAIR2,
        BASS_SPEAKER_PAIR11 = BASS_SPEAKER_PAIR10 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR12 = BASS_SPEAKER_PAIR8 | BASS_SPEAKER_PAIR4,
        BASS_SPEAKER_PAIR13 = BASS_SPEAKER_PAIR12 | BASS_SPEAKER_PAIR1,
        BASS_SPEAKER_PAIR14 = BASS_SPEAKER_PAIR12 | BASS_SPEAKER_PAIR2,
        BASS_SPEAKER_PAIR15 = BASS_SPEAKER_PAIR14 | BASS_SPEAKER_PAIR1,
        BASS_UNICODE = -2147483648,
        BASS_SAMPLE_OVER_VOL = 65536,
        BASS_SAMPLE_OVER_POS = BASS_STREAM_PRESCAN,
        BASS_SAMPLE_OVER_DIST = BASS_SAMPLE_OVER_POS | BASS_SAMPLE_OVER_VOL,
        BASS_WV_STEREO = 4194304,
        BASS_AC3_DOWNMIX_2 = 512,
        BASS_AC3_DOWNMIX_4 = 1024,
        BASS_AC3_DOWNMIX_DOLBY = BASS_AC3_DOWNMIX_4 | BASS_AC3_DOWNMIX_2,
        BASS_AC3_DYNAMIC_RANGE = 2048,
        BASS_AAC_STEREO = BASS_WV_STEREO,
        BASS_MIXER_END = BASS_SAMPLE_OVER_VOL,
        BASS_MIXER_PAUSE = BASS_SAMPLE_OVER_POS,
        BASS_MIXER_NONSTOP = BASS_MIXER_PAUSE,
        BASS_MIXER_RESUME = 4096,
        BASS_MIXER_POSEX = 8192,
        BASS_MIXER_LIMIT = 16384,
        BASS_MIXER_MATRIX = BASS_MIXER_END,
        BASS_MIXER_DOWNMIX = BASS_AAC_STEREO,
        BASS_MIXER_NORAMPIN = BASS_STREAM_STATUS,
        BASS_MIXER_FILTER = BASS_MIXER_RESUME,
        BASS_SPLIT_SLAVE = BASS_MIXER_FILTER,
        BASS_MIXER_BUFFER = BASS_MIXER_POSEX,
        BASS_CD_SUBCHANNEL = BASS_AC3_DOWNMIX_2,
        BASS_CD_SUBCHANNEL_NOHW = BASS_AC3_DOWNMIX_4,
        BASS_CD_C2ERRORS = BASS_AC3_DYNAMIC_RANGE,
        BASS_MIDI_DECAYEND = BASS_SPLIT_SLAVE,
        BASS_MIDI_NOFX = BASS_MIXER_BUFFER,
        BASS_MIDI_DECAYSEEK = BASS_MIXER_LIMIT,
        BASS_MIDI_SINCINTER = BASS_MIXER_NORAMPIN,
        BASS_MIDI_FONT_MMAP = BASS_MIXER_NONSTOP,
        BASS_FX_FREESOURCE = BASS_MIXER_MATRIX,
        BASS_FX_BPM_BKGRND = BASS_SAMPLE_8BITS,
        BASS_FX_BPM_MULT2 = BASS_SAMPLE_MONO,
        BASS_MUSIC_FLOAT = BASS_SAMPLE_FLOAT,
        BASS_MUSIC_MONO = BASS_FX_BPM_MULT2,
        BASS_MUSIC_LOOP = BASS_SAMPLE_LOOP,
        BASS_MUSIC_3D = BASS_SAMPLE_3D,
        BASS_MUSIC_FX = BASS_SAMPLE_FX,
        BASS_MUSIC_AUTOFREE = BASS_STREAM_AUTOFREE,
        BASS_MUSIC_DECODE = BASS_STREAM_DECODE,
        BASS_MUSIC_PRESCAN = BASS_MIDI_FONT_MMAP,
        BASS_MUSIC_RAMP = BASS_CD_SUBCHANNEL,
        BASS_MUSIC_RAMPS = BASS_CD_SUBCHANNEL_NOHW,
        BASS_MUSIC_SURROUND = BASS_CD_C2ERRORS,
        BASS_MUSIC_SURROUND2 = BASS_MIDI_DECAYEND,
        BASS_MUSIC_FT2MOD = BASS_MIDI_NOFX,
        BASS_MUSIC_PT1MOD = BASS_MIDI_DECAYSEEK,
        BASS_MUSIC_NONINTER = BASS_FX_FREESOURCE,
        BASS_MUSIC_SINCINTER = BASS_MIDI_SINCINTER,
        BASS_MUSIC_POSRESET = BASS_RECORD_PAUSE,
        BASS_MUSIC_POSRESETEX = BASS_MIXER_DOWNMIX,
        BASS_MUSIC_STOPBACK = BASS_STREAM_RESTRATE,
        BASS_MUSIC_NOSAMPLE = BASS_STREAM_BLOCK,
        BASS_DSHOW_STREAM_MIX = BASS_SPEAKER_PAIR1,
        BASS_DSHOW_NOAUDIO_PROC = BASS_MUSIC_STOPBACK,
        BASS_DSHOW_STREAM_AUTODVD = BASS_SPEAKER_PAIR4,
        BASS_DSHOW_STREAM_LOOP = BASS_SPEAKER_PAIR8,
        BASS_DSHOW_STREAM_VIDEOPROC = BASS_MUSIC_PRESCAN,
    }

    [Flags]
    public enum BASSChannelType
    {
        BASS_CTYPE_UNKNOWN = 0,
        BASS_CTYPE_SAMPLE = 1,
        BASS_CTYPE_RECORD = 2,
        BASS_CTYPE_MUSIC_MO3 = 256,
        BASS_CTYPE_STREAM = 65536,
        BASS_CTYPE_STREAM_OGG = BASS_CTYPE_STREAM | BASS_CTYPE_RECORD,
        BASS_CTYPE_STREAM_MP1 = BASS_CTYPE_STREAM_OGG | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_MP2 = 65540,
        BASS_CTYPE_STREAM_MP3 = BASS_CTYPE_STREAM_MP2 | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_AIFF = BASS_CTYPE_STREAM_MP2 | BASS_CTYPE_RECORD,
        BASS_CTYPE_STREAM_CA = BASS_CTYPE_STREAM_AIFF | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_MF = 65544,
        BASS_CTYPE_STREAM_MIXER = 67584,
        BASS_CTYPE_STREAM_SPLIT = BASS_CTYPE_STREAM_MIXER | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_WAV = 262144,
        BASS_CTYPE_STREAM_WAV_PCM = BASS_CTYPE_STREAM_WAV | BASS_CTYPE_STREAM | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_WAV_FLOAT = BASS_CTYPE_STREAM_WAV_PCM | BASS_CTYPE_RECORD,
        BASS_CTYPE_MUSIC_MOD = 131072,
        BASS_CTYPE_MUSIC_MTM = BASS_CTYPE_MUSIC_MOD | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_MUSIC_S3M = BASS_CTYPE_MUSIC_MOD | BASS_CTYPE_RECORD,
        BASS_CTYPE_MUSIC_XM = BASS_CTYPE_MUSIC_S3M | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_MUSIC_IT = 131076,
        BASS_CTYPE_STREAM_WV = 66816,
        BASS_CTYPE_STREAM_WV_H = BASS_CTYPE_STREAM_WV | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_WV_L = BASS_CTYPE_STREAM_WV | BASS_CTYPE_RECORD,
        BASS_CTYPE_STREAM_WV_LH = BASS_CTYPE_STREAM_WV_L | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_CD = 66048,
        BASS_CTYPE_STREAM_WMA = BASS_CTYPE_STREAM_CD | BASS_CTYPE_MUSIC_MO3,
        BASS_CTYPE_STREAM_WMA_MP3 = BASS_CTYPE_STREAM_WMA | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_FLAC = BASS_CTYPE_STREAM_MIXER | BASS_CTYPE_MUSIC_MO3,
        BASS_CTYPE_STREAM_FLAC_OGG = BASS_CTYPE_STREAM_FLAC | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_OFR = 67072,
        BASS_CTYPE_STREAM_APE = BASS_CTYPE_STREAM_OFR | BASS_CTYPE_MUSIC_MO3,
        BASS_CTYPE_STREAM_MPC = 68096,
        BASS_CTYPE_STREAM_AAC = BASS_CTYPE_STREAM_MPC | BASS_CTYPE_MUSIC_MO3,
        BASS_CTYPE_STREAM_MP4 = BASS_CTYPE_STREAM_AAC | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_SPX = 68608,
        BASS_CTYPE_STREAM_ALAC = 69120,
        BASS_CTYPE_STREAM_TTA = BASS_CTYPE_STREAM_ALAC | BASS_CTYPE_MUSIC_MO3,
        BASS_CTYPE_STREAM_AC3 = 69632,
        BASS_CTYPE_STREAM_OPUS = 70144,
        BASS_CTYPE_STREAM_WINAMP = 66560,
        BASS_CTYPE_STREAM_MIDI = 68864,
        BASS_CTYPE_STREAM_ADX = 126976,
        BASS_CTYPE_STREAM_AIX = BASS_CTYPE_STREAM_ADX | BASS_CTYPE_SAMPLE,
        BASS_CTYPE_STREAM_VIDEO = BASS_CTYPE_STREAM_AC3 | BASS_CTYPE_MUSIC_MO3,
    }

    #endregion
}

#endif