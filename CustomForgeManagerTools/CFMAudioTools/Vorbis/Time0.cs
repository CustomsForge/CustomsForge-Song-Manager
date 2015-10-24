using System;
using Ogg;

namespace Vorbis 
{
	class Time0 : FuncTime
	{
		override public void pack(Object i, csBuffer opb){}
        override public Object unpack(Info vi, csBuffer opb) { return ""; }
        override public Object look(DspState vd, InfoMode mi, Object i) { return ""; }
		override public void free_info(Object i){}
		override public void free_look(Object i){}
        override public int forward(Block vb, Object i) { return 0; }
        override public int inverse(Block vb, Object i, float[] fin, float[] fout) { return 0; }
	}
}