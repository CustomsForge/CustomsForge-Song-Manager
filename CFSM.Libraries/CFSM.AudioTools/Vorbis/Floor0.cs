using System;
using CFSM.AudioTools.Ogg;

namespace CFSM.AudioTools.Vorbis
{
    internal class Floor0 : FuncFloor
    {
        public override void pack(Object i, csBuffer opb)
        {
            InfoFloor0 info = (InfoFloor0) i;
            opb.write(info.order, 8);
            opb.write(info.rate, 16);
            opb.write(info.barkmap, 16);
            opb.write(info.ampbits, 6);
            opb.write(info.ampdB, 8);
            opb.write(info.numbooks - 1, 4);
            for (int j = 0; j < info.numbooks; j++)
                opb.write(info.books[j], 8);
        }

        public override Object unpack(Info vi, csBuffer opb)
        {
            InfoFloor0 info = new InfoFloor0() {order = opb.read(8), rate = opb.read(16), barkmap = opb.read(16), ampbits = opb.read(6), ampdB = opb.read(8), numbooks = opb.read(4) + 1};

            if ((info.order < 1) || (info.rate < 1) || (info.barkmap < 1) || (info.numbooks < 1))
            {
                //free_info(info);
                return (null);
            }

            for (int j = 0; j < info.numbooks; j++)
            {
                info.books[j] = opb.read(8);
                if (info.books[j] < 0 || info.books[j] >= vi.books)
                {
                    //free_info(info);
                    return (null);
                }
            }
            return (info);
            //  err_out:
            //    free_info(info);
            //    return(NULL);
        }

        public override Object look(DspState vd, InfoMode mi, Object i)
        {
            float scale;

            Info vi = vd.vi;
            InfoFloor0 info = (InfoFloor0) i;
            LookFloor0 look = new LookFloor0() {m = info.order, n = vi.blocksizes[mi.blockflag]/2, ln = info.barkmap, vi = info};
            look.lpclook.init(look.ln, look.m);

            // we choose a scaling constant so that:
            //  floor(bark(rate/2-1)*C)=mapped-1
            // floor(bark(rate/2)*C)=mapped
            scale = look.ln/(float) toBARK((float) (info.rate/2.0));

            // the mapping from a linear scale to a smaller bark scale is
            // straightforward.  We do *not* make sure that the linear mapping
            // does not skip bark-scale bins; the decoder simply skips them and
            // the encoder may do what it wishes in filling them.  They're
            // necessary in some mapping combinations to keep the scale spacing
            // accurate
            look.linearmap = new int[look.n];

            for (int j = 0; j < look.n; j++)
            {
                int val = (int) Math.Floor(toBARK((float) ((info.rate/2.0)/look.n*j))*scale); // bark numbers represent band edges
                if (val >= look.ln) val = look.ln; // guard against the approximation
                look.linearmap[j] = val;
            }
            return look;
        }

        private static double toBARK(float f)
        {
            double a, b, c;
            a = 13.1*Math.Atan(0.00074*f);
            b = 2.24*Math.Atan(f*f*1.85e-8);
            c = 1.0e-4*f;

            return (a + b + c);
        }

        private object state(object i)
        {
            EchstateFloor0 state = new EchstateFloor0();
            InfoFloor0 info = (InfoFloor0) i;

            // a safe size if usually too big (dim==1)
            state.codewords = new int[info.order];
            state.curve = new float[info.barkmap];
            state.frameno = -1;
            return (state);
        }

        public override void free_info(Object i)
        {
        }

        public override void free_look(Object i)
        {
        }

        public override void free_state(Object vs)
        {
        }

        public override int forward(Block vb, Object i, float[] fin, float[] fout, Object vs)
        {
            return 0;
        }

        private float[] lsp = null;

        private int inverse(Block vb, Object i, float[] fout)
        {
            //System.err.println("Floor0.inverse "+i.getClass()+"]");
            LookFloor0 look = (LookFloor0) i;
            InfoFloor0 info = look.vi;
            int ampraw = vb.opb.read(info.ampbits);
            if (ampraw > 0)
            {
                // also handles the -1 out of data case
                int maxval = (1 << info.ampbits) - 1;
                float amp = (float) ampraw/maxval*info.ampdB;
                int booknum = vb.opb.read(ilog(info.numbooks));

                if (booknum != -1 && booknum < info.numbooks)
                {
                    lock (this)
                    {
                        if (lsp == null || lsp.Length < look.m)
                        {
                            lsp = new float[look.m];
                        }
                        else
                        {
                            for (int j = 0; j < look.m; j++) lsp[j] = 0.0f;
                        }

                        CodeBook b = vb.vd.fullbooks[info.books[booknum]];
                        float last = 0.0f;

                        //memset(out,0,sizeof(float)*look->m);
                        for (int j = 0; j < look.m; j++) fout[j] = 0.0f;

                        for (int j = 0; j < look.m; j += b.dim)
                        {
                            if (b.decodevs(lsp, j, vb.opb, 1, -1) == -1)
                            {
                                //goto eop;
                                // memset(out,0,sizeof(float)*look->n);
                                for (int k = 0; k < look.n; k++) fout[k] = 0.0f;
                                return (0);
                            }
                        }
                        for (int j = 0; j < look.m;)
                        {
                            for (int k = 0; k < b.dim; k++, j++) lsp[j] += last;
                            last = lsp[j - 1];
                        }
                        // take the coefficients back to a spectral envelope curve
                        /*
                        lsp_to_lpc(out,out,look.m); 
                        lpc_to_curve(out,out,amp,look,"",0);
                        for(int j=0;j<look.n;j++){
                          out[j]=fromdB(out[j]-info.ampdB);
                        }
                        */
                        Lsp.lsp_to_curve(fout, look.linearmap, look.n, look.ln, lsp, look.m, amp, info.ampdB);

                        return (1);
                    }
                }
            }
            //  eop:
            //    memset(out,0,sizeof(float)*look->n);
            return (0);
        }

        public override Object inverse1(Block vb, Object i, Object memo)
        {
            //System.err.println("Floor0.inverse "+i.getClass()+"]");
            LookFloor0 look = (LookFloor0) i;
            InfoFloor0 info = look.vi;
            float[] lsp = null;
            if (memo is float[])
            {
                lsp = (float[]) memo;
            }

            int ampraw = vb.opb.read(info.ampbits);
            if (ampraw > 0)
            {
                // also handles the -1 out of data case
                int maxval = (1 << info.ampbits) - 1;
                float amp = (float) ampraw/maxval*info.ampdB;
                int booknum = vb.opb.read(ilog(info.numbooks));

                if (booknum != -1 && booknum < info.numbooks)
                {
                    CodeBook b = vb.vd.fullbooks[info.books[booknum]];
                    float last = 0.0f;

                    if (lsp == null || lsp.Length < look.m + 1)
                    {
                        lsp = new float[look.m + 1];
                    }
                    else
                    {
                        for (int j = 0; j < lsp.Length; j++) lsp[j] = 0.0f;
                    }

                    for (int j = 0; j < look.m; j += b.dim)
                    {
                        if (b.decodev_set(lsp, j, vb.opb, b.dim) == -1)
                        {
                            //goto eop;
                            return (null);
                        }
                    }

                    for (int j = 0; j < look.m;)
                    {
                        for (int k = 0; k < b.dim; k++, j++) lsp[j] += last;
                        last = lsp[j - 1];
                    }
                    lsp[look.m] = amp;
                    return (lsp);
                }
            }
            //  eop:
            return (null);
        }

        public override int inverse2(Block vb, Object i, Object memo, float[] fout)
        {
            //System.err.println("Floor0.inverse "+i.getClass()+"]");
            LookFloor0 look = (LookFloor0) i;
            InfoFloor0 info = look.vi;

            if (memo != null)
            {
                float[] lsp = (float[]) memo;
                float amp = lsp[look.m];

                Lsp.lsp_to_curve(fout, look.linearmap, look.n, look.ln, lsp, look.m, amp, info.ampdB);
                return (1);
            }
            //  eop:
            //    memset(out,0,sizeof(float)*look->n);
            for (int j = 0; j < look.n; j++)
            {
                fout[j] = 0.0f;
            }
            return (0);
        }

        private static float fromdB(float x)
        {
            return (float) (Math.Exp((x)*.11512925));
        }

        private static int ilog(int v)
        {
            int ret = 0;
            while (v != 0)
            {
                ret++;
                v = (int) ((uint) v >> 1);
            }
            return (ret);
        }

        private static void lsp_to_lpc(float[] lsp, float[] lpc, int m)
        {
            int i, j, m2 = m/2;
            float[] O = new float[m2];
            float[] E = new float[m2];
            float A;
            float[] Ae = new float[m2 + 1];
            float[] Ao = new float[m2 + 1];
            float B;
            float[] Be = new float[m2];
            float[] Bo = new float[m2];
            float temp;

            // even/odd roots setup
            for (i = 0; i < m2; i++)
            {
                O[i] = (float) (-2.0*Math.Cos(lsp[i*2]));
                E[i] = (float) (-2.0*Math.Cos(lsp[i*2 + 1]));
            }

            // set up impulse response
            for (j = 0; j < m2; j++)
            {
                Ae[j] = 0.0f;
                Ao[j] = 1.0f;
                Be[j] = 0.0f;
                Bo[j] = 1.0f;
            }
            Ao[j] = 1.0f;
            Ae[j] = 1.0f;

            // run impulse response
            for (i = 1; i < m + 1; i++)
            {
                A = B = 0.0f;
                for (j = 0; j < m2; j++)
                {
                    temp = O[j]*Ao[j] + Ae[j];
                    Ae[j] = Ao[j];
                    Ao[j] = A;
                    A += temp;

                    temp = E[j]*Bo[j] + Be[j];
                    Be[j] = Bo[j];
                    Bo[j] = B;
                    B += temp;
                }
                lpc[i - 1] = (A + Ao[j] + B - Ae[j])/2;
                Ao[j] = A;
                Ae[j] = B;
            }
        }

        private static void lpc_to_curve(float[] curve, float[] lpc, float amp, LookFloor0 l, String name, int frameno)
        {
            // l->m+1 must be less than l->ln, but guard in case we get a bad stream
            float[] lcurve = new float[Math.Max(l.ln*2, l.m*2 + 2)];

            if (amp == 0)
            {
                //memset(curve,0,sizeof(float)*l->n);
                for (int j = 0; j < l.n; j++) curve[j] = 0.0f;
                return;
            }
            l.lpclook.lpc_to_curve(lcurve, lpc, amp);

            for (int i = 0; i < l.n; i++) curve[i] = lcurve[l.linearmap[i]];
        }
    }

    internal class InfoFloor0
    {
        internal int order;
        internal int rate;
        internal int barkmap;

        internal int ampbits;
        internal int ampdB;

        internal int numbooks; // <= 16
        internal int[] books = new int[16];
    }

    internal class LookFloor0
    {
        internal int n;
        internal int ln;
        internal int m;
        internal int[] linearmap;

        internal InfoFloor0 vi;
        internal Lpc lpclook = new Lpc();
    }

    internal class EchstateFloor0
    {
        internal int[] codewords;
        internal float[] curve;
        internal long frameno;
        //internal long codes;
    }
}