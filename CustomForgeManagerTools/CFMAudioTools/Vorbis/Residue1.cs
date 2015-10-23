using System;

namespace CFMAudioTools.Vorbis 
{
	class Residue1 : Residue0
	{
        new int forward(Block vb, Object vl, float[][] fin, int ch)
        {
            return 0;
        }

        override public int inverse(Block vb, Object vl, float[][] fin, int[] nonzero, int ch)
        {
            //System.err.println("Residue0.inverse");
            int used = 0;
            for (int i = 0; i < ch; i++)
            {
                if (nonzero[i] != 0)
                {
                    fin[used++] = fin[i];
                }
            }
            if (used != 0)
            {
                return (_01inverse(vb, vl, fin, used, 1));
            }
            else
            {
                return 0;
            }
        }
	}
}