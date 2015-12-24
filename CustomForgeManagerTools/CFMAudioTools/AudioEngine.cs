using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CFMAudioTools
{
    public abstract class AudioEngine
    {
        static AudioEngine DefaultEngine;

        public static AudioEngine GetDefaultEngine()
        {
            if (DefaultEngine == null)
                DefaultEngine = new BassEngine();
            return DefaultEngine;
        }

        public abstract bool OpenAudioStream(Stream stream);
        public virtual bool OpenAudioFile(string Filename)
        {
            using (var fs = File.OpenRead(Filename))
                return OpenAudioStream(fs);
        }

        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();
        public abstract double GetSongLength();
        public abstract double GetSongPos();
        public abstract void Seek(double position);

        public abstract bool IsPlaying();
        public abstract bool IsPaused();
        public abstract bool IsLoaded();
        public int GetSongCompletedPercentage()
        {
            if (!IsPlaying())
                return 0;

            return Convert.ToInt32((GetSongPos() / GetSongLength()) * 100);
        }

        public string GetSongPosition()
        {
            if (!IsPlaying())
                return "00:00";

            var seconds = Convert.ToInt32(GetSongPos()) % 60;
            var minutes = Convert.ToInt32(GetSongPos()) / 60;
            return String.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    internal class BassEngine : AudioEngine
    {
        int FHandle = 0;
        static bool bassinit = false;
        IntPtr SongBytes = IntPtr.Zero;
        // RS2014 user 48kHz DVD quality playback vs 44.1 kHz (44100) CD quality playback
        // tried 48000 here and it sounds bad in long songs
        private static int sampleRate = 44100;

        static BassEngine()
        {
            if (!Bass.BASS_Init(-1, sampleRate, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                Trace.WriteLine("Unable to initilize bass library.");
                bassinit = false;
            }
            else
                bassinit = true;
        }

        static byte[] StreamToBytes(Stream input)
        {
            if (input is MemoryStream)
            {
                return ((MemoryStream)input).ToArray();
            }
            else
                using (MemoryStream ms = new MemoryStream())
                {
                    input.CopyTo(ms);
                    return ms.ToArray();
                }
        }

        public override bool IsPlaying()
        {
            if (!bassinit || FHandle == 0)
                return false;
            return Bass.BASS_ChannelIsActive(FHandle) == BASSActive.BASS_ACTIVE_PLAYING;
        }

        public override bool IsLoaded()
        {
            if (!bassinit || FHandle == 0)
                return false;
            return true;
        }

        public override bool IsPaused()
        {
            if (!bassinit || FHandle == 0)
                return false;
            return Bass.BASS_ChannelIsActive(FHandle) == BASSActive.BASS_ACTIVE_PAUSED;

        }

        public override void Pause()
        {
            if (IsLoaded())
            {
                if (IsPlaying())
                    Bass.BASS_ChannelPause(FHandle);
                else
                    Bass.BASS_ChannelPlay(FHandle, false);

            }
        }


        public override bool OpenAudioFile(string Filename)
        {
            if (!bassinit)
                return false;
            //close the old stream
            if (FHandle != 0)
            {
                if (SongBytes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(SongBytes);
                    SongBytes = IntPtr.Zero;
                }

                Bass.BASS_ChannelStop(FHandle);
                Bass.BASS_StreamFree(FHandle);
                FHandle = 0;
            }
            FHandle = Bass.BASS_StreamCreateFile(Filename, 0, 0, BASSFlag.BASS_DEFAULT);
            if (FHandle == 0)
                Debug.WriteLine(Bass.BASS_ErrorGetCode());

            return FHandle != 0;
        }

        public override bool OpenAudioStream(Stream stream)
        {
            if (!bassinit)
                return false;
            //close the old stream
            if (FHandle != 0)
            {
                if (SongBytes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(SongBytes);
                    SongBytes = IntPtr.Zero;
                }

                Bass.BASS_ChannelStop(FHandle);
                Bass.BASS_StreamFree(FHandle);
                FHandle = 0;
            }
            stream.Position = 0;
            var bytes = StreamToBytes(stream);
            SongBytes = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, SongBytes, bytes.Length);
            FHandle = Bass.BASS_StreamCreateFile(SongBytes, 0, 0, BASSFlag.BASS_DEFAULT);
            var result = FHandle != 0;
            if (!result)
            {
                Debug.WriteLine(Bass.BASS_ErrorGetCode());
                Marshal.FreeHGlobal(SongBytes);
                SongBytes = IntPtr.Zero;
            }


            return result;
        }

        private bool checkInit()
        {
            return bassinit && FHandle != 0;
        }

        public override void Play()
        {
            if (!checkInit())
                return;
            Bass.BASS_ChannelPlay(FHandle, false);
        }

        public override void Stop()
        {
            if (!checkInit())
                return;
            Bass.BASS_ChannelStop(FHandle);
        }


        public override double GetSongLength()
        {
            if (!checkInit())
                return 0;
            return Bass.BASS_ChannelBytes2Seconds(FHandle, Bass.BASS_ChannelGetLength(FHandle));
        }

        public override double GetSongPos()
        {
            if (!checkInit())
                return 0;
            return Bass.BASS_ChannelBytes2Seconds(FHandle, Bass.BASS_ChannelGetPosition(FHandle));
        }



        public override void Seek(double seconds)
        {
            if (!checkInit())
                return;
            Bass.BASS_ChannelSetPosition(FHandle, seconds);
        }
    }
}
