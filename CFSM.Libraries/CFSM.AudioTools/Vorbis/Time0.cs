using System;
using CFSM.AudioTools.Ogg;

namespace CFSM.AudioTools.Vorbis
{
    internal class Time0 : FuncTime
    {
        public override void pack(Object i, csBuffer opb)
        {
        }

        public override Object unpack(Info vi, csBuffer opb)
        {
            return "";
        }

        public override Object look(DspState vd, InfoMode mi, Object i)
        {
            return "";
        }

        public override void free_info(Object i)
        {
        }

        public override void free_look(Object i)
        {
        }

        public override int forward(Block vb, Object i)
        {
            return 0;
        }

        public override int inverse(Block vb, Object i, float[] fin, float[] fout)
        {
            return 0;
        }
    }
}