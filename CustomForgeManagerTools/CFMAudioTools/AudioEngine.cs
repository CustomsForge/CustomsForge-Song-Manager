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
        public abstract double GetSongLength();
        public abstract double GetSongPos();
        public abstract void Seek(double position);

    }

    internal class BassEngine : AudioEngine
    {
        int FHandle = 0;
        static bool bassinit = false;
        IntPtr SongBytes = IntPtr.Zero;

        static BassEngine()
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
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
