using Ogg;

namespace Vorbis 
{
	public class Block
	{
		///necessary stream state for linking to the framing abstraction
		internal float[][] pcm=new float[0][]; // this is a pointer into local storage
		internal csBuffer opb=new csBuffer();
  
		internal int lW;
		internal int W;
		internal int nW;
		internal int pcmend;
		internal int mode;

		internal int eofflag;
		internal long granulepos;
		internal long sequence;
		internal DspState vd; // For read-only access of configuration


		// bitmetrics for the frame
		internal int glue_bits;
		internal int time_bits;
		internal int floor_bits;
		internal int res_bits;

        public Block(DspState vd)
        {
            this.vd = vd;
            if (vd.analysisp != 0)
            {
                opb.writeinit();
            }
        }

        public void init(DspState vd)
        {
            this.vd = vd;
        }


		public int clear()
		{
			if(vd!=null)
			{
				if(vd.analysisp!=0)
				{
					opb.writeclear();
				}
			}
			return(0);
		}

        public int synthesis(Packet op)
        {
            Info vi = vd.vi;
            opb.readinit(op.packet_base, op.packet, op.bytes);

            // Check the packet type
            if (opb.read(1) != 0)
            {
                // Oops.  This is not an audio data packet
                return (-1);
            }

            // read our mode and pre/post windowsize
            int _mode = opb.read(vd.modebits);
            if (_mode == -1) return (-1);

            mode = _mode;
            W = vi.mode_param[mode].blockflag;
            if (W != 0)
            {
                lW = opb.read(1);
                nW = opb.read(1);
                if (nW == -1) return (-1);
            }
            else
            {
                lW = 0;
                nW = 0;
            }

            // more setup
            granulepos = op.granulepos;
            sequence = op.packetno - 3; // first block is third packet
            eofflag = op.e_o_s;

            // alloc pcm passback storage
            pcmend = vi.blocksizes[W];
            //pcm=alloc(vi.channels);
            if (pcm.Length < vi.channels)
            {
                pcm = new float[vi.channels][];
            }
            for (int i = 0; i < vi.channels; i++)
            {
                if (pcm[i] == null || pcm[i].Length < pcmend)
                {
                    pcm[i] = new float[pcmend];
                }
                else
                {
                    for (int j = 0; j < pcmend; j++) { pcm[i][j] = 0; }
                }
            }

            // unpack_header enforces range checking
            int type = vi.map_type[vi.mode_param[mode].mapping];
            return (FuncMapping.mapping_P[type].inverse(this, vd.mode[mode]));
        }
	}
}