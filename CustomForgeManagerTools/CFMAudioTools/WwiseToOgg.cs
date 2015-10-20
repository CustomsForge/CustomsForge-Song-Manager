using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CFMAudioTools.Ogg;
using MiscUtil.Conversion;
using MiscUtil.IO;

namespace CFMAudioTools
{
    public class WwiseToOgg
    {
        protected string FCodebooksName = "packed_codebooks_aoTuV_603";
        //int FDataOffset = -1;
        //Int32 FFmtOffset = -1;
        //Int32 FCueOffset = -1;
        // Int32 FSetupPacketOffset = 0;
        protected Stream FInStream, FOutStream;
        protected Stream FRealOutStream;
        protected bool _little_endian = true;
        protected bool _header_triad_present = false;
        protected UInt16 _channels = 0;
        protected uint _sample_rate = 0, _avg_bytes_per_second = 0;
        protected long _file_size = -1;
        protected EndianBinaryReader FStreamReader;
        protected int _riff_size = -1;
        public string CodebooksName { get { return FCodebooksName; } set { FCodebooksName = value; } }

        // public delegate void read32(Byte[] b, int start, Int32 v);
        //internal read32 FRead32;
        protected int _fmt_offset = -1;
        protected int _fmt_size = -1;
        protected int _cue_offset = -1;
        protected int _cue_size = -1;
        protected int _LIST_offset = -1;
        protected int _LIST_size = -1;
        protected int _smpl_offset = -1;
        protected int _smpl_size = -1;
        protected int _vorb_offset = -1;
        protected int _vorb_size = -1;
        protected int _data_offset = -1;
        protected int _data_size = -1;
        protected ushort _ext_unk;
        protected int _subtype;
        protected uint _cue_count;
        protected uint _loop_count;
        protected uint _loop_start;
        protected uint _loop_end;
        protected uint _sample_count;
        protected bool _no_granule;
        protected bool _mod_packets;
        protected int _setup_packet_offset;
        protected int _first_audio_packet_offset;
        protected bool _old_packet_headers;
        protected int _uid;
        protected byte _blocksize_0_pow;
        protected byte _blocksize_1_pow;
        protected bool _inline_codebooks;
        protected bool _full_setup = false;
        protected uint mode_bits;
        
        public WwiseToOgg(Stream inStream, Stream outStream)
        {
            FInStream = inStream;
            FOutStream = new MemoryStream(); //outStream;
            FRealOutStream = outStream;
            InitilizeStream();
        }

        protected void InitilizeStream()
        {
            _file_size = FInStream.Length;
            byte[] riffhead = new byte[4];
            FInStream.Read(riffhead, 0, 4);
            var rs = Encoding.ASCII.GetString(riffhead);
            if (rs != "RIFX")
            {
                if (rs != "RIFF")
                {
                    throw new WwiseToOggException("missing RIFF");
                }
                _little_endian = true;

            }
            else
                _little_endian = false;

            if (_little_endian)
                FStreamReader = new EndianBinaryReader(EndianBitConverter.Little, FInStream);
            else
                FStreamReader = new EndianBinaryReader(EndianBitConverter.Big, FInStream);

            //     FRead32 = MyBitWriter.write_32_le;

            _riff_size = FStreamReader.ReadInt32() + 8;
            if (_riff_size > _file_size)
                throw new WwiseToOggException("RIFF truncated");

            riffhead = new byte[4];
            FInStream.Read(riffhead, 0, 4);
            rs = Encoding.ASCII.GetString(riffhead);
            if (rs != "WAVE")
                throw new WwiseToOggException("missing WAVE");

            //process format
            int chunk_offset = 12;
            while (chunk_offset < _riff_size)
            {
                FInStream.Seek(chunk_offset, SeekOrigin.Begin);

                if (chunk_offset + 8 > _riff_size)
                    throw new WwiseToOggException("RIFF truncated");

                byte[] chunk_type = new byte[4];
                FInStream.Read(chunk_type, 0, 4);
                string cts = Encoding.ASCII.GetString(chunk_type); // fmt magicID
                int chunk_size = FStreamReader.ReadInt32();

                switch (cts)
                {
                    case "fmt ":
                        _fmt_offset = chunk_offset + 8;
                        _fmt_size = chunk_size;
                        break;
                    case "cue ":
                        _cue_offset = chunk_offset + 8;
                        _cue_size = chunk_size;
                        break;
                    case "LIST":
                        _LIST_offset = chunk_offset + 8;
                        _LIST_size = chunk_size;
                        break;
                    case "smpl":
                        _smpl_offset = chunk_offset + 8;
                        _smpl_size = chunk_size;
                        break;
                    case "vorb":
                        _vorb_offset = chunk_offset + 8;
                        _vorb_size = chunk_size;
                        break;
                    case "data":
                        _data_offset = chunk_offset + 8;
                        _data_size = chunk_size;
                        break;
                }
                chunk_offset = chunk_offset + 8 + chunk_size;
            }

            if (chunk_offset > _riff_size) throw new WwiseToOggException("chunk truncated");

            if (-1 == _fmt_offset && -1 == _data_offset) throw new WwiseToOggException("expected fmt, data chunks");

            // read fmt
            if (-1 == _vorb_offset && 0x42 != _fmt_size) throw new WwiseToOggException("expected 0x42 fmt if vorb missing");

            if (-1 != _vorb_offset && 0x28 != _fmt_size && 0x18 != _fmt_size) throw new WwiseToOggException("bad fmt size");

            if (-1 == _vorb_offset && 0x42 == _fmt_size)
            {
                // fake it out
                _vorb_offset = _fmt_offset + 0x18;
            }

            FInStream.Seek(_fmt_offset, SeekOrigin.Begin);
            if (FStreamReader.ReadUInt16() != 0xFFFF)
                throw new WwiseToOggException("bad codec id");

            _channels = FStreamReader.ReadUInt16();
            _sample_rate = FStreamReader.ReadUInt32();
            _avg_bytes_per_second = FStreamReader.ReadUInt32();

            if (0U != FStreamReader.ReadUInt16()) throw new WwiseToOggException("bad block align");
            if (0U != FStreamReader.ReadUInt16()) throw new WwiseToOggException("expected 0 bps");
            if (_fmt_size - 0x12 != FStreamReader.ReadUInt16()) throw new WwiseToOggException("bad extra fmt length");



            _ext_unk = FStreamReader.ReadUInt16();//wSamplesPerBlock
            _subtype = FStreamReader.ReadInt32();//dwChannelMask

            if (_fmt_size == 0x28)
            {
                // byte[] whoknowsbuf = new byte[16];
                byte[] whoknowsbuf_check = new byte[16] { 1, 0, 0, 0, 0, 0, 0x10, 0, 0x80, 0, 0, 0xAA, 0, 0x38, 0x9b, 0x71 };
                byte[] whoknowsbuf = FStreamReader.ReadBytes(16);

                if (!whoknowsbuf.SequenceEqual(whoknowsbuf_check))
                {
                    throw new WwiseToOggException("expected signature in extra fmt?");
                }
            }

            if (-1 != _cue_offset)
            {
                //#if 0
                if (0x1c != _cue_size) throw new WwiseToOggException("bad cue size");
                //#endif
                FInStream.Seek(_cue_offset, SeekOrigin.Begin);

                _cue_count = FStreamReader.ReadUInt32();
            }

            // read LIST
            if (-1 != _LIST_offset)
            {
                if (4 != _LIST_size) throw new WwiseToOggException("bad LIST size");
                FInStream.Seek(_LIST_offset, SeekOrigin.Begin);
                byte[] adtlbuf = FStreamReader.ReadBytes(4);
                string ab = Encoding.ASCII.GetString(adtlbuf);
                if (ab != "adtl")
                    throw new WwiseToOggException("expected only adtl in LIST");
            }

            if (-1 != _smpl_offset)
            {
                FInStream.Seek(_smpl_offset + 0x1C, SeekOrigin.Begin);
                _loop_count = FStreamReader.ReadUInt32();

                if (1 != _loop_count) throw new WwiseToOggException("expected one loop");

                FInStream.Seek(_smpl_offset + 0x2c, SeekOrigin.Begin);
                _loop_start = FStreamReader.ReadUInt32();
                _loop_end = FStreamReader.ReadUInt32();
            }

            switch (_vorb_size)
            {
                case -1:
                case 0x28:
                case 0x2A:
                case 0x2C:
                case 0x32:
                case 0x34:
                    FInStream.Seek(_vorb_offset + 0x00, SeekOrigin.Begin);
                    break;

                default:
                    throw new WwiseToOggException("bad vorb size");
            }
            _sample_count = FStreamReader.ReadUInt32();

            switch (_vorb_size)
            {
                case -1:
                case 0x2A:
                    {
                        _no_granule = true;

                        FInStream.Seek(_vorb_offset + 0x4, SeekOrigin.Begin);
                        Int64 mod_signal = FStreamReader.ReadInt32();

                        // seems to be 0xD9 when _mod_packets should be set
                        // also seen 0xCB, 0xBC, 0xB2
                        // possible signal bits are 10000000 high, 01000000 low
                        if (0x4A != mod_signal && 0x4B != mod_signal && 0x69 != mod_signal && 0x70 != mod_signal)
                        {
                            _mod_packets = true;
                        }
                        FInStream.Seek(_vorb_offset + 0x10, SeekOrigin.Begin);
                        break;
                    }

                default:
                    FInStream.Seek(_vorb_offset + 0x18, SeekOrigin.Begin);
                    break;
            }

            _setup_packet_offset = FStreamReader.ReadInt32();
            _first_audio_packet_offset = FStreamReader.ReadInt32();

            switch (_vorb_size)
            {
                case -1:
                case 0x2A:
                    FInStream.Seek(_vorb_offset + 0x24, SeekOrigin.Begin);
                    break;

                case 0x32:
                case 0x34:
                    FInStream.Seek(_vorb_offset + 0x2C, SeekOrigin.Begin);
                    break;
            }

            switch (_vorb_size)
            {
                case 0x28:
                case 0x2C:
                    // ok to leave _uid, _blocksize_0_pow and _blocksize_1_pow unset
                    _header_triad_present = true;
                    _old_packet_headers = true;
                    break;

                case -1:
                case 0x2A:
                case 0x32:
                case 0x34:
                    _uid = FStreamReader.ReadInt32();
                    _blocksize_0_pow = FStreamReader.ReadByte();
                    _blocksize_1_pow = FStreamReader.ReadByte();
                    break;
            }

            // check/set loops now that we know total sample count
            if (0 != _loop_count)
            {
                if (_loop_end == 0)
                {
                    _loop_end = _sample_count;
                }
                else
                {
                    _loop_end = _loop_end + 1;
                }

                if (_loop_start >= _sample_count || _loop_end > _sample_count || _loop_start > _loop_end)
                    throw new WwiseToOggException("loops out of range");
            }

            // check subtype now that we know the vorb info
            // this is clearly just the channel layout
            //switch (_subtype)
            //{
            //    case 4:     /* 1 channel, no seek table */
            //    case 3:     /* 2 channels */
            //    case 0x33:  /* 4 channels */
            //    case 0x37:  /* 5 channels, seek or not */
            //    case 0x3b:  /* 5 channels, no seek table */
            //    case 0x3f:  /* 6 channels, no seek table */
            //        break;
            //    default:
            //        //throw Parse_error_str("unknown subtype");
            //        break;
            //}


        }
        public virtual bool ConvertToOgg(bool full_setup = false, bool _revorb = true)
        {
            _full_setup = full_setup;

            ogStream os = new ogStream(FOutStream);
            List<bool> mode_blockflag = new List<bool>();
            bool prev_blockflag = false;

            if (_header_triad_present)
                throw new WwiseToOggException("currently can not convert an file with a triad present.");
            //  generate_ogg_header_with_triad(os);
            else
                generate_ogg_header(os, mode_blockflag);


            //Audio Pages:
            {
                int offset = _data_offset + _first_audio_packet_offset;

                Int32 size, granule;
                int packet_header_size, packet_payload_offset, next_offset;
                //   int lastbs = 0;
                //  int xOffset = 0;
                while (offset < _data_offset + _data_size)
                {
                    if (_old_packet_headers)
                    {
                        Packet8 ap = new Packet8(FInStream, offset, _little_endian);
                        packet_header_size = ap.HeaderSize;
                        size = ap.size;
                        packet_payload_offset = ap.Offset;
                        granule = ap.granule;
                        next_offset = ap.NextOffset;
                    }
                    else
                    {
                        Packet6 ap = new Packet6(FInStream, offset, _little_endian, _no_granule);
                        packet_header_size = ap.HeaderSize;
                        size = ap.size;
                        packet_payload_offset = ap.Offset;
                        granule = ap.granule;
                        next_offset = ap.NextOffset;
                    }

                    //int bs = 0;
                    //if (lastbs != 0)
                    //    granule += (lastbs + bs) / 4;
                    //lastbs = next_offset;




                    if (offset + packet_header_size > _data_offset + _data_size)
                    {
                        throw new WwiseToOggException("page header truncated");
                    }

                    offset = packet_payload_offset;
                    FInStream.Seek(offset, SeekOrigin.Begin);
                    os.set_granule(Convert.ToInt64(granule));

                    if (_mod_packets)
                    {
                        if (mode_blockflag.Count == 0)
                        {
                            throw new WwiseToOggException("didn't load mode_blockflag");
                        }

                        Bit_uint packet_type = new Bit_uint(1);
                        os.write(packet_type);

                        Bit_uint mode_number_p = null;
                        Bit_uint remainder_p = null;

                        {
                            // collect mode number from first byte

                            bitStream ss = new bitStream(FInStream);

                            // IN/OUT: N bit mode number (max 6 bits)
                            mode_number_p = ss.Read(mode_bits);
                            os.write(mode_number_p);

                            // IN: remaining bits of first (input) byte
                            remainder_p = ss.Read(8U - mode_bits);
                        }

                        if (mode_blockflag[Convert.ToInt32(mode_number_p)])
                        {
                            // long window, peek at next frame

                            FInStream.Seek(next_offset, SeekOrigin.Begin);
                            bool next_blockflag = false;
                            if (next_offset + packet_header_size <= _data_offset + _data_size)
                            {

                                // mod_packets always goes with 6-byte headers
                                Packet6 audio_packet = new Packet6(FInStream, next_offset, _little_endian, _no_granule);
                                var next_packet_size = audio_packet.size;
                                if (next_packet_size > 0)
                                {
                                    FInStream.Seek(audio_packet.Offset, SeekOrigin.Begin);

                                    bitStream ss = new bitStream(FInStream);
                                    var next_mode_number = ss.Read(mode_bits);


                                    next_blockflag = mode_blockflag[Convert.ToInt32(next_mode_number)];
                                }
                            }

                            // OUT: previous window type bit
                            var prev_window_type = new Bit_uint(1, Convert.ToUInt32(prev_blockflag));
                            os.write(prev_window_type);

                            // OUT: next window type bit
                            var next_window_type = new Bit_uint(1, Convert.ToUInt32(next_blockflag));
                            os.write(next_window_type);

                            // fix seek for rest of stream
                            FInStream.Seek(offset + 1, SeekOrigin.Begin);
                        }

                        prev_blockflag = mode_blockflag[Convert.ToInt32(mode_number_p)];
                        //delete mode_number_p;

                        // OUT: remaining bits of first (input) byte
                        os.write(remainder_p);


                    }
                    else
                    {
                        int v = FInStream.ReadByte();
                        if (v < 0)
                            throw new WwiseToOggException("file truncated");

                        os.write(new Bit_uint(8, v));
                    }

                    // remainder of packet
                    for (int i = 1; i < size; i++)
                    {
                        int v = FInStream.ReadByte();
                        if (v < 0)
                            throw new WwiseToOggException("file truncated");

                        os.write(new Bit_uint(8, v));
                    }
                    offset = next_offset;
                    os.flush_page(false, (offset == _data_offset + _data_size));
                }
                if (offset > _data_offset + _data_size)
                    throw new WwiseToOggException("page truncated");
            }
            if (!_revorb)
            {
                FOutStream.Position = 0;
                FOutStream.CopyTo(FRealOutStream);
                return true;
            }
            else
                return revorb();
        }

        private bool revorb()
        {
            FOutStream.Position = 0;
            return new OggRevorb(FOutStream, FRealOutStream).Execute();
        }

        private void cout(string s)
        {
            Console.WriteLine(s);
        }

        public void PrintInfo()
        {
            cout(_little_endian ? "RIFF WAVE" : "RIFX WAVE");
            cout(_channels + " channels");
            cout(String.Format("{0} Hz {1}: bps", _sample_rate, _avg_bytes_per_second * 8));
            cout(_sample_count + " samples");

            if (0 != _loop_count)
            {
                cout(string.Format("loop from {0} to {1}", _loop_start, _loop_end));
            }

            if (_old_packet_headers)
            {
                cout("- 8 byte (old) packet headers");
            }
            else if (_no_granule)
            {
                cout("- 6 byte packet headers, no granule");
            }
            else
            {
                cout("- 6 byte packet headers");
            }

            if (_header_triad_present)
            {
                cout("- Vorbis header triad present");
            }

            if (_full_setup || _header_triad_present)
            {
                cout("- full setup header");
            }
            else
            {
                cout("- stripped setup header");
            }

            if (_inline_codebooks || _header_triad_present)
            {
                cout("- inline codebooks");
            }
            else
            {
                cout("- external codebooks");
            }

            if (_mod_packets)
            {
                cout("- shortened Vorbis packets");
            }
            else
            {
                cout("- standard Vorbis packets");
            }

        }


        internal bool generate_setup_packet(ogStream os, List<bool> mode_blockflag)
        {
            os.write(new Vorbis_packet_header(5));
            //  vhead.LeftShift(os);

            Packet6 setup_packet = new Packet6(FInStream, _data_offset + _setup_packet_offset, _little_endian, _no_granule);

            FInStream.Seek(setup_packet.Offset, SeekOrigin.Begin);

            if (setup_packet.granule != 0)
                throw new WwiseToOggException("setup packet granule != 0");

            bitStream ss = new bitStream(FInStream);
            Bit_uint codebook_count_less1 = ss.Read(8);

            UInt32 codebook_count = codebook_count_less1 + 1;
            os.write(codebook_count_less1);

            if (_inline_codebooks)
            {
                codebook_library cbl = new codebook_library();

                for (int i = 0; i < codebook_count; i++)
                {
                    if (_full_setup)
                    {
                        cbl.copy(ss, os);
                    }
                    else
                    {
                        cbl.rebuild(ss, 0, os);
                    }
                }
            }
            else
            {
                byte[] data = null; //AudioTools.Properties.Resources.packed_codebooks_aoTuV_603;
                //byte[] data = AudioTools.Properties.Resources.packed_codebooks;


                if (String.IsNullOrEmpty(FCodebooksName))
                    FCodebooksName = "packed_codebooks_aoTuV_603";

                if (File.Exists(FCodebooksName))
                {
                    using (FileStream fcn = File.OpenRead(FCodebooksName))
                    {
                        data = new byte[fcn.Length];
                        fcn.Read(data, 0, data.Length);
                    }
                }
                else
                {
                    data = (byte[])Properties.Resources.ResourceManager.GetObject(FCodebooksName);
                }
                if (data == null)
                    data = Properties.Resources.packed_codebooks_aoTuV_603;

                using (MemoryStream ms = new MemoryStream(data))
                {
                    ms.Position = 0;
                    codebook_library cbl = new codebook_library(ms);
                    ms.Position = 0;
                    for (int i = 0; i < codebook_count; i++)
                    {
                        Bit_uint codebook_id = new Bit_uint(10);
                        ss.Read(codebook_id);

                        try
                        {
                            cbl.rebuild(Convert.ToInt32(codebook_id), os);
                        }
                        catch (codebook_library.Invalid_id E)
                        {

                            if (codebook_id == 0x342)
                            {
                                var codebook_identifier = ss.Read(14);
                                if (codebook_identifier == 0x1590)
                                {
                                    // starts with BCV, probably --full-setup
                                    throw new WwiseToOggException(
                                        "invalid codebook id 0x342");//, try --full-setup");
                                }
                            }
                            // just an invalid codebook
                            throw E;
                        }
                    }
                }
            }

            Bit_uint time_count_less1 = new Bit_uint(6);
            os.write(time_count_less1);
            Bit_uint dummy_time_value = new Bit_uint(16);
            os.write(dummy_time_value);

            if (_full_setup)
            {

                while (ss.get_total_bits_read() < setup_packet.size * 8u)
                {
                    os.write(ss.Read(1));
                }
            }
            else    // _full_setup
            {
                // Always floor type 1

                var floor_count_less1 = ss.Read(6);
                uint floor_count = floor_count_less1 + 1;
                os.write(floor_count_less1);

                // rebuild floors
                for (int i = 0; i < floor_count; i++)
                {
                    os.write(new Bit_uint(16, 1));
                    var floor1_part = ss.Read(5);
                    os.write(floor1_part);

                    uint[] floor1_partition_class_list = new uint[floor1_part];
                    uint minclass = 0;
                    for (int j = 0; j < floor1_part; j++)
                    {
                        var floor1_partition_class = ss.Read(4);
                        os.write(floor1_partition_class);

                        floor1_partition_class_list[j] = floor1_partition_class;

                        if (floor1_partition_class > minclass)
                            minclass = floor1_partition_class;
                    }

                    uint[] floor1_class_dimensions_list = new uint[minclass + 1];

                    for (int j = 0; j <= minclass; j++)
                    {
                        var class_dimensions_less1 = ss.Read(3);
                        os.write(class_dimensions_less1);

                        floor1_class_dimensions_list[j] = class_dimensions_less1 + 1;

                        var class_subclasses = ss.Read(2);
                        os.write(class_subclasses);

                        if (class_subclasses != 0)
                        {
                            var masterbook = ss.Read(8);
                            os.write(masterbook);

                            if (masterbook >= codebook_count)
                                throw new WwiseToOggException("invalid floor1 masterbook");
                        }

                        for (int k = 0; k < (1U << class_subclasses.totalI); k++)
                        {
                            var subclass_book_plus1 = ss.Read(8);
                            os.write(subclass_book_plus1);

                            int subclass_book = Convert.ToInt32(subclass_book_plus1) - 1;
                            if (subclass_book >= 0 && Convert.ToUInt32(subclass_book) >= codebook_count)
                                throw new WwiseToOggException("invalid floor1 subclass book");
                        }
                    }
                    os.write(ss.Read(2));//floor1_multiplier_less1
                    var rangebits = ss.Read(4);
                    os.write(rangebits);
                    uint rb = rangebits;

                    for (int j = 0; j < floor1_part; j++)
                    {
                        var current_class_number = floor1_partition_class_list[j];
                        for (int k = 0; k < floor1_class_dimensions_list[current_class_number]; k++)
                        {
                            os.write(ss.Read(rb));
                        }
                    }
                }
                var residue_count_less1 = ss.Read(6);
                var residue_count = residue_count_less1 + 1;
                os.write(residue_count_less1);

                for (int i = 0; i < residue_count; i++)
                {
                    var residue_type = ss.Read(2);
                    os.write(new Bit_uint(16, residue_type));
                    if (residue_type > 2)
                        throw new WwiseToOggException("invalid residue type");

                    var residue_begin = ss.Read(24);
                    var residue_end = ss.Read(24);
                    var residue_partition_size_less1 = ss.Read(24);
                    var residue_classifications_less1 = ss.Read(6);
                    var residue_classbook = ss.Read(8);

                    uint residue_classifications = residue_classifications_less1 + 1;

                    os.write(residue_begin);
                    os.write(residue_end);
                    os.write(residue_partition_size_less1);
                    os.write(residue_classifications_less1);
                    os.write(residue_classbook);
                    if (residue_classbook >= codebook_count) throw new WwiseToOggException("invalid residue classbook");

                    uint[] residue_cascade = new uint[residue_classifications];

                    for (int j = 0; j < residue_classifications; j++)
                    {
                        var high_bits = new Bit_uint(5);
                        var low_bits = ss.Read(3);
                        os.write(low_bits);

                        var bitflag = ss.Read(1);
                        os.write(bitflag);
                        if (bitflag != 0)
                        {
                            ss.Read(high_bits);
                            os.write(high_bits);
                        }

                        residue_cascade[j] = high_bits * 8 + low_bits;
                    }

                    for (int j = 0; j < residue_classifications; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if ((Convert.ToInt32(residue_cascade[j]) & (1 << k)) != 0)
                            {
                                var residue_book = ss.Read(8);
                                os.write(residue_book);

                                if (residue_book >= codebook_count) throw new WwiseToOggException("invalid residue book");
                            }
                        }
                    }
                }


                // mapping count
                var mapping_count_less1 = ss.Read(6);
                uint mapping_count = mapping_count_less1 + 1;
                os.write(mapping_count_less1);

                for (int i = 0; i < mapping_count; i++)
                {
                    // always mapping type 0, the only one
                    //var mapping_type = new Bit_uint(16);

                    os.write(new Bit_uint(16));//mapping_type);

                    var submaps_flag = ss.Read(1);
                    os.write(submaps_flag);

                    uint submaps = 1;
                    if (submaps_flag != 0)
                    {
                        var submaps_less1 = ss.Read(4);
                        submaps = submaps_less1 + 1;
                        os.write(submaps_less1);
                    }

                    var square_polar_flag = ss.Read(1);
                    os.write(square_polar_flag);

                    if (square_polar_flag != 0)
                    {
                        var coupling_steps_less1 = ss.Read(8);
                        uint coupling_steps = coupling_steps_less1 + 1;
                        os.write(coupling_steps_less1);

                        for (int j = 0; j < coupling_steps; j++)
                        {
                            var magnitude = ss.Read(Convert.ToUInt32(_channels - 1));
                            var angle = ss.Read(Convert.ToUInt32(_channels - 1));
                            os.write(magnitude);
                            os.write(angle);
                            if (angle == magnitude || magnitude >= _channels || angle >= _channels)
                                throw new WwiseToOggException("invalid coupling");
                        }
                    }

                    // a rare reserved field not removed by Ak!
                    var mapping_reserved = ss.Read(2);
                    os.write(mapping_reserved);
                    if (0 != mapping_reserved)
                        throw new WwiseToOggException("mapping reserved field nonzero");

                    if (submaps > 1)
                    {
                        for (int j = 0; j < _channels; j++)
                        {
                            var mapping_mux = ss.Read(4);
                            os.write(mapping_mux);

                            if (mapping_mux >= submaps)
                                throw new WwiseToOggException("mapping_mux >= submaps");
                        }
                    }

                    for (int j = 0; j < submaps; j++)
                    {
                        // Another! Unused time domain transform configuration placeholder!
                        var time_config = ss.Read(8);
                        os.write(time_config);

                        var floor_number = ss.Read(8);
                        os.write(floor_number);
                        if (floor_number >= floor_count)
                            throw new WwiseToOggException("invalid floor mapping");

                        var residue_number = ss.Read(8);
                        os.write(residue_number);
                        if (residue_number >= residue_count)
                            throw new WwiseToOggException("invalid residue mapping");
                    }
                }

                // mode count
                var mode_count_less1 = ss.Read(6);
                uint mode_count = mode_count_less1 + 1;
                os.write(mode_count_less1);
                mode_blockflag.Clear();
                //mode_blockflag = new bool[mode_count];
                mode_bits = (mode_count - 1);

                //cout << mode_count << " modes" << endl;

                for (int i = 0; i < mode_count; i++)
                {
                    var block_flag = ss.Read(1);
                    os.write(block_flag);

                    mode_blockflag.Add(block_flag != 0);

                    // only 0 valid for windowtype and transformtype
                    var windowtype = new Bit_uint(16);
                    var transformtype = new Bit_uint(16);

                    os.write(windowtype);
                    os.write(transformtype);

                    var mapping = ss.Read(8);
                    os.write(mapping);
                    if (mapping >= mapping_count)
                        throw new WwiseToOggException("invalid mode mapping");
                }
                os.write(new Bit_uint(1, 1));//framing

            }
            os.flush_page();//false,false,false);
            if ((ss.get_total_bits_read() + 7) / 8 != setup_packet.size)
                throw new WwiseToOggException("didn't read exactly setup packet");

            if (setup_packet.NextOffset != _data_offset + _first_audio_packet_offset)
                throw new WwiseToOggException("first audio packet doesn't follow setup packet");

            return true;
        }

        internal bool generate_ogg_header(ogStream os, List<bool> mode_blockflag)
        {
            //generate id packet ..this is correct
            {
                os.write(new Vorbis_packet_header(1));
                os.write(new Bit_uint(32, 0));//version
                os.write(new Bit_uint(8, _channels));
                os.write(new Bit_uint(32, _sample_rate));
                os.write(new Bit_uint(32, 0));//bitrate_max
                os.write(new Bit_uint(32, _avg_bytes_per_second * 8));//bitrate_nominal
                os.write(new Bit_uint(32, 0));//bitrate_minimum
                os.write(new Bit_uint(4, _blocksize_0_pow));
                os.write(new Bit_uint(4, _blocksize_1_pow));
                os.write(new Bit_uint(1, 1));
                os.flush_page();
            }


            // generate comment packet
            {
                os.write(new Vorbis_packet_header(3));

                // const string vendor = "converted from Audiokinetic Wwise by df_oggconvert v1.0 ";
                const string vendor = "converted from Wwise by dfaudiolib v0.8";
                Bit_uint vendorsize = new Bit_uint(32, Convert.ToUInt32(vendor.Length));
                os.write(vendorsize);

                for (int i = 0; i < vendorsize; i++)
                {
                    byte b = Convert.ToByte(vendor[i]);
                    os.write(new Bit_uint(8, Convert.ToUInt32(b)));
                }

                if (_loop_count == 0)
                {
                    Bit_uint user_comment_count = new Bit_uint(32, 0);
                    os.write(user_comment_count);
                }
                else
                {
                    Bit_uint user_comment_count = new Bit_uint(32, 2);
                    os.write(user_comment_count);

                    string loop_start_str = "LoopStart=" + _loop_start;
                    string loop_end_str = "LoopEnd=" + _loop_end;

                    Bit_uint loop_start_comment_length = new Bit_uint(32, loop_start_str.Length);
                    os.write(loop_start_comment_length);

                    for (int i = 0; i < loop_start_comment_length; i++)
                    {
                        byte b = Convert.ToByte(loop_start_str[i]);
                        Bit_uint c = new Bit_uint(8, Convert.ToUInt32(b));
                        os.write(c);
                    }

                    Bit_uint loop_end_comment_length = new Bit_uint(32, loop_end_str.Length);
                    os.write(loop_end_comment_length);

                    for (int i = 0; i < loop_end_comment_length; i++)
                    {
                        byte b = Convert.ToByte(loop_end_str[i]);
                        Bit_uint c = new Bit_uint(8, Convert.ToUInt32(b));
                        os.write(c);
                    }
                }

                Bit_uint framing = new Bit_uint(1, 1);
                os.write(framing);
                os.flush_page();
            }
            generate_setup_packet(os, mode_blockflag);



            return true;

            // generate setup packet

        }

        //private int ilog(UInt32 v)
        //{
        //    int ret = 0;
        //    while (v != 0)
        //    {
        //        ret++;
        //        v >>= 1;
        //    }
        //    return ret;
        //}

        //private bool generate_ogg_header_with_triad(ogStream os)
        //{
        //    int offset = FDataOffset + FSetupPacketOffset;
        //    Packet8 information_packet = new Packet8(FInStream, offset, _little_endian);
        //    var size = information_packet.size;
        //    if (information_packet.granule != 0)
        //        throw new WwiseToOggException("information packet granule != 0");
        //    FInStream.Seek(information_packet.Offset, SeekOrigin.Begin);
        //    return false;
        //}

        
    }

    internal class codebook_library
    {
        private byte[] codebook_data;
        internal int codebook_count { get; private set; }
        internal uint[] codebook_offsets { get; private set; }

        public codebook_library()
        {
            codebook_count = 0;
        }

        public codebook_library(Stream astream)
        {
            var file_size = astream.Length;
            astream.Seek(file_size - 4, SeekOrigin.Begin);

            var offset_offset = MyBitWriter.read_32_le(astream);
            codebook_count = Convert.ToInt32((file_size - offset_offset) / 4);
            codebook_data = new byte[offset_offset];
            codebook_offsets = new uint[codebook_count];

            astream.Position = 0;

            for (int i = 0; i < offset_offset; i++)
                codebook_data[i] = (byte)astream.ReadByte();

            for (int i = 0; i < codebook_count; i++)
                codebook_offsets[i] = MyBitWriter.read_32_le(astream);
        }

        private int ilog(UInt32 v)
        {
            int ret = 0;
            while (v != 0)
            {
                ret++;
                v >>= 1;
            }
            return ret;
        }


        //int ilog(unsigned int v){
        //  int ret=0;
        //  while(v){
        //    ret++;
        //    v>>=1;
        //  }
        //  return(ret);
        //}

        private uint _book_maptype1_quantvals(uint entries, uint dimensions)
        {
            int bits = ilog(entries);
            var x = Convert.ToInt32((bits - 1) * (dimensions - 1) / dimensions);
            var vals = entries >> x;
            while (true)
            {
                uint acc = 1;
                uint acc1 = 1;
                for (int i = 0; i < dimensions; i++)
                {
                    acc *= vals;
                    acc1 *= vals + 1;
                }
                if (acc <= entries && acc1 > entries)
                    return (vals);
                else
                    if (acc > entries)
                        vals--;
                    else
                        vals++;

            }
        }

        public void copy(bitStream bis, ogStream bos)
        {
            Bit_uint id = new Bit_uint(24);
            Bit_uint dimensions = new Bit_uint(16);
            Bit_uint entries = new Bit_uint(24);
            bis.Read(id);
            bis.Read(dimensions);
            bis.Read(entries);

            if (0x564342 != id)
                throw new InvalidIDException(id);

            bos.write(id);

            dimensions = new Bit_uint(16, dimensions);
            entries = new Bit_uint(24, entries);
            bos.write(dimensions);
            bos.write(entries);

            Bit_uint ordered = new Bit_uint(1);
            bis.Read(ordered);
            bos.write(ordered);

            if (ordered != 0)
            {
                Bit_uint initial_length = new Bit_uint(5);
                bis.Read(initial_length);
                bos.write(initial_length);

                uint current_entry = 0;
                while (current_entry < entries)
                {
                    Bit_uint number = new Bit_uint(Convert.ToUInt32(ilog(entries - current_entry)));
                    bis.Read(number);
                    bos.write(number);
                    current_entry += number;
                }
                if (current_entry > entries) throw new WwiseToOggException("current_entry out of range");


            }
            else
            {
                Bit_uint sparse = new Bit_uint(1);
                bis.Read(sparse);
                bos.write(sparse);

                for (int i = 0; i < entries; i++)
                {
                    bool present_bool = true;

                    if (sparse != 0)
                    {
                        //        /* IN/OUT 1 bit sparse presence flag */
                        Bit_uint present = new Bit_uint(1);
                        bis.Read(present);
                        bos.write(present);

                        present_bool = (0 != present);

                    }
                    if (present_bool)
                    {
                        //        /* IN/OUT: 5 bit codeword length-1 */
                        var Bit_uintcodeword_length = new Bit_uint(5);
                        bis.Read(Bit_uintcodeword_length);
                        bos.write(Bit_uintcodeword_length);

                    }
                }
            }//done with lengths

            Bit_uint lookup_type = new Bit_uint(4);
            bis.Read(lookup_type);
            bos.write(lookup_type);

            if (lookup_type == 0)
            {

            }
            else
                if (lookup_type == 1)
                {
                    Bit_uint min = new Bit_uint(32);
                    Bit_uint max = new Bit_uint(32);
                    Bit_uint value_length = new Bit_uint(4);
                    Bit_uint sequence_flag = new Bit_uint(1);
                    bis.Read(min);
                    bis.Read(max);
                    bis.Read(value_length);
                    bis.Read(sequence_flag);

                    bos.write(min);
                    bos.write(max);
                    bos.write(value_length);
                    bos.write(sequence_flag);

                    uint quantvals = _book_maptype1_quantvals(entries, dimensions);

                    for (int i = 0; i < quantvals; i++)
                    {
                        Bit_uint val = new Bit_uint(value_length + 1);
                        bis.Read(val);
                        bos.write(val);
                    }
                }
                else if (2 == lookup_type)
                {
                    throw new WwiseToOggException("didn't expect lookup type 2");
                }
                else
                {
                    throw new WwiseToOggException("invalid lookup type");
                }
        }

        public class Invalid_id : WwiseToOggException
        {
            public int id { get; private set; }

            public Invalid_id(int ID)
                : base("Invalid ID :" + ID)
            {
                id = ID;
            }
        }

        int get_codebook_size(int i)
        {
            if (i >= codebook_count - 1 || i < 0) return -1;
            return Convert.ToInt32(codebook_offsets[i + 1] - codebook_offsets[i]);
        }

        byte[] get_codebook(int i)
        {
            if (i >= codebook_count - 1 || i < 0) return null;
            int x = get_codebook_size(i);
            byte[] result = new byte[x];

            for (int z = 0; z < x; z++)
                result[z] = codebook_data[codebook_offsets[i] + z];
            return result;
        }

        public void rebuild(int i, ogStream bos)
        {
            byte[] cb = get_codebook(i);
            if (cb[0] == 0 || cb == null)
                throw new Invalid_id(i);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(cb, 0, cb.Length);
                ms.Position = 0;
                bitStream bs = new bitStream(ms);
                rebuild(bs, Convert.ToUInt32(cb.Length), bos);
            }
        }

        public void rebuild(int i, csBuffer bos)
        {
            byte[] cb = get_codebook(i);
            if (cb[0] == 0 || cb == null)
                throw new Invalid_id(i);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(cb, 0, cb.Length);
                ms.Position = 0;
                bitStream bs = new bitStream(ms);
                rebuild(bs, Convert.ToUInt32(cb.Length), bos);
            }
        }

        public void rebuild(bitStream bis, uint cb_size, csBuffer buffer)
        {
            Bit_uint dimensions = new Bit_uint(4);
            Bit_uint entries = new Bit_uint(14);
            bis.Read(dimensions);
            bis.Read(entries);

            new Bit_uint(24, 0x564342).Write(buffer);
            new Bit_uint(16, dimensions).Write(buffer);
            new Bit_uint(24, entries).Write(buffer);

            Bit_uint ordered = bis.Read(1);
            ordered.Write(buffer);

            if (ordered != 0)
            {
                Bit_uint initial_length = bis.Read(5);
                initial_length.Write(buffer);
                //initial_length.LeftShift(bos);

                uint current_entry = 0;
                while (current_entry < entries)
                {
                    Bit_uint number = bis.Read(entries - current_entry);
                    number.Write(buffer);
                    current_entry += number;
                }
                if (current_entry > entries)
                    throw new WwiseToOggException("current_entry out of range");
            }
            else
            {
                Bit_uint codeword_length_length = new Bit_uint(3);
                Bit_uint sparse = new Bit_uint(1);
                bis.Read(codeword_length_length);
                bis.Read(sparse);
                if (codeword_length_length == 0 || codeword_length_length > 5)
                {
                    throw new WwiseToOggException("nonsense codeword length");
                }
                sparse.Write(buffer);
                for (int i = 0; i < entries; i++)
                {
                    bool present_bool = true;
                    if (sparse != 0)
                    {
                        Bit_uint present = new Bit_uint(1);
                        bis.Read(present);
                        present.Write(buffer);
                        present_bool = (0 != present);

                    }
                    if (present_bool)
                    {
                        //        /* IN/OUT: 5 bit codeword length-1 */
                        var codeword_length = new Bit_uint(codeword_length_length);
                        bis.Read(codeword_length);
                        new Bit_uint(5, codeword_length).Write(buffer);
                    }
                }
            }

            Bit_uint lookup_type = bis.Read(1);
            new Bit_uint(4, lookup_type).Write(buffer);

            if (lookup_type == 0)
            {

            }
            else
                if (lookup_type == 1)
                {

                    Bit_uint min = new Bit_uint(32);
                    Bit_uint max = new Bit_uint(32);
                    Bit_uint value_length = new Bit_uint(4);
                    Bit_uint sequence_flag = new Bit_uint(1);
                    bis.Read(min);
                    bis.Read(max);
                    bis.Read(value_length);
                    bis.Read(sequence_flag);

                    min.Write(buffer);
                    max.Write(buffer);
                    value_length.Write(buffer);
                    sequence_flag.Write(buffer);

                    uint quantvals = _book_maptype1_quantvals(entries, dimensions);

                    for (int i = 0; i < quantvals; i++)
                    {
                        Bit_uint val = new Bit_uint(value_length + 1);
                        bis.Read(val);
                        val.Write(buffer);
                    }


                }
                else if (lookup_type == 2)
                {
                    throw new WwiseToOggException("didn't expect lookup type 2");
                }
                else
                {
                    throw new WwiseToOggException("invalid lookup type");
                }

            var totalread = bis.get_total_bits_read() / 8 + 1;
            if (cb_size != 0 && totalread != cb_size)
            {
                throw new WwiseToOggException(String.Format("{0} {1}", cb_size, totalread));
            }

        }

        //int ilog(uint v){
        //  int ret=0;
        //  while(v != 0){
        //    ret++;
        //    v>>=1;
        //  }
        //  return(ret);
        //}


        public void rebuild(bitStream bis, uint cb_size, ogStream bos)
        {
            Bit_uint dimensions = new Bit_uint(4);
            Bit_uint entries = new Bit_uint(14);
            bis.Read(dimensions);
            bis.Read(entries);
            bos.write(new Bit_uint(24, 0x564342));
            bos.write(new Bit_uint(16, dimensions));
            bos.write(new Bit_uint(24, entries));

            Bit_uint ordered = bis.Read(1);
            bos.write(ordered);

            if (ordered != 0)
            {
                Bit_uint initial_length = bis.Read(5);
                bos.write(initial_length);
                //initial_length.LeftShift(bos);

                uint current_entry = 0;
                while (current_entry < entries)
                {
                    var x = ilog(entries - current_entry);

                    Bit_uint number = bis.Read(Convert.ToUInt32(x));
                    bos.write(number);
                    current_entry += number;
                }
                if (current_entry > entries)
                    throw new WwiseToOggException("current_entry out of range");
            }
            else
            {
                Bit_uint codeword_length_length = new Bit_uint(3);
                Bit_uint sparse = new Bit_uint(1);
                bis.Read(codeword_length_length);
                bis.Read(sparse);
                if (codeword_length_length == 0 || codeword_length_length > 5)
                {
                    throw new WwiseToOggException("nonsense codeword length");
                }
                bos.write(sparse);
                for (int i = 0; i < entries; i++)
                {
                    bool present_bool = true;
                    if (sparse != 0)
                    {
                        Bit_uint present = new Bit_uint(1);
                        bis.Read(present);
                        bos.write(present);
                        present_bool = (0 != present);

                    }
                    if (present_bool)
                    {
                        //        /* IN/OUT: 5 bit codeword length-1 */
                        var codeword_length = new Bit_uint(codeword_length_length);
                        bis.Read(codeword_length);
                        bos.write(new Bit_uint(5, codeword_length));
                    }
                }
            }

            Bit_uint lookup_type = bis.Read(1);
            bos.write(new Bit_uint(4, lookup_type));

            if (lookup_type == 0)
            {

            }
            else
                if (lookup_type == 1)
                {

                    Bit_uint min = new Bit_uint(32);
                    Bit_uint max = new Bit_uint(32);
                    Bit_uint value_length = new Bit_uint(4);
                    Bit_uint sequence_flag = new Bit_uint(1);
                    bis.Read(min);
                    bis.Read(max);
                    bis.Read(value_length);
                    bis.Read(sequence_flag);

                    bos.write(min);
                    bos.write(max);
                    bos.write(value_length);
                    bos.write(sequence_flag);

                    uint quantvals = _book_maptype1_quantvals(entries, dimensions);

                    for (int i = 0; i < quantvals; i++)
                    {
                        Bit_uint val = new Bit_uint(value_length + 1);
                        bis.Read(val);
                        bos.write(val);
                    }


                }
                else if (lookup_type == 2)
                {
                    throw new WwiseToOggException("didn't expect lookup type 2");
                }
                else
                {
                    throw new WwiseToOggException("invalid lookup type");
                }

            var totalread = bis.get_total_bits_read() / 8 + 1;
            if (cb_size != 0 && totalread != cb_size)
            {
                throw new WwiseToOggException(String.Format("{0} {1}", cb_size, totalread));
            }
        }

    }

    internal class ogStream : _bitStream
    {
        protected int streamserial = 666;
        protected int bits_stored = 0;
        protected const int header_bytes = 27;
        protected const int max_segments = 255;
        protected const int segment_size = 255;

        protected int payload_bytes;
        protected Int64 granule = 0;
        protected int seqno = 0;
        protected bool first = true, continued = false;
        protected Byte[] page_buffer;

        public ogStream(Stream astream)
            : base(astream)
        {
            page_buffer = new Byte[header_bytes + max_segments + segment_size * max_segments];
        }

        internal void write(Vorbis_packet_header v)
        {
            write(new Bit_uint(8, v.ilength));
            for (int i = 0; i < 6; i++)
                write(new Bit_uint(8, Convert.ToUInt32(v.vorbis_str[i])));
        }
        public void set_granule(Int64 g)
        {
            granule = g;
        }


        public void write(Bit_uint v)
        {
            for (int i = 0; i < v.bitsize; i++)
            {
                put_bit((v & (1U << i)) != 0);
            }
        }

        //public void write(byte[] b)
        //{

        //}



        public void put_bit(bool bit)
        {
            if (bit)
                bit_buffer |= 1 << bits_stored;
            bits_stored++;
            if (bits_stored == 8)
                flush_bits();
        }

        protected virtual void flush_bits()
        {
            if (bits_stored != 0)
            {
                if (payload_bytes == segment_size * max_segments)
                {
                    //  throw new WwiseToOggException("ran out of space in an Ogg packet");
                    flush_page(true);
                }
                byte[] x = BitConverter.GetBytes(bit_buffer);
                page_buffer[header_bytes + max_segments + payload_bytes] = x[0];
                page_buffer[header_bytes + max_segments + payload_bytes + 1] = x[1];
                page_buffer[header_bytes + max_segments + payload_bytes + 2] = x[2];
                page_buffer[header_bytes + max_segments + payload_bytes + 3] = x[3];
                payload_bytes++;
                bits_stored = 0;
                bit_buffer = 0;
            }
        }

        //  int ASegments = 0;

        public virtual byte[] getBytes()
        {
            if (payload_bytes != segment_size * max_segments)
            {
                flush_bits();
            }
            if (payload_bytes != 0)
            {
                var result = new byte[payload_bytes];
                for (int i = 0; i < payload_bytes; i++)
                {
                    result[i] = page_buffer[header_bytes + max_segments + i];
                }
            }
            return new byte[0];
        }

        public virtual void flush_page(bool next_continued = false, bool last = false, bool writeHeader = true)
        {
            if (payload_bytes != segment_size * max_segments)
            {
                flush_bits();
            }
            if (payload_bytes != 0)
            {

                int segments = ((payload_bytes + segment_size) / segment_size);  // intentionally round up
                if (segments == max_segments + 1) segments = max_segments; // at max eschews the final 0
                //ASegments = 0;
                // move payload back
                int count = header_bytes + segments + payload_bytes;
                for (int i = 0; i < payload_bytes; i++)
                {
                    page_buffer[header_bytes + segments + i] =
                        page_buffer[header_bytes + max_segments + i];
                }

                page_buffer[0] = Convert.ToByte('O');
                page_buffer[1] = Convert.ToByte('g');
                page_buffer[2] = Convert.ToByte('g');
                page_buffer[3] = Convert.ToByte('S');
                page_buffer[4] = 0; // stream_structure_version
                page_buffer[5] = Convert.ToByte((continued ? 1 : 0) | (first ? 2 : 0) | (last ? 4 : 0));
                page_buffer.write_64_le(6, granule);
                page_buffer.write_32(14, streamserial);       // stream serial number
                page_buffer.write_32(18, seqno);   // page sequence number
                page_buffer.write_32(22, 0);       // checksum (0 for now)
                page_buffer[26] = Convert.ToByte(segments);             // segment count



                // lacing values
                for (int i = 0, bytes_left = payload_bytes; i < segments; i++)
                {
                    if (bytes_left >= segment_size)
                    {
                        bytes_left -= segment_size;
                        page_buffer[27 + i] = segment_size;
                    }
                    else
                    {
                        page_buffer[27 + i] = Convert.ToByte(bytes_left);
                    }
                }
                var checksum = CRC32.checksum(page_buffer, count);

                MyBitWriter.write_32(page_buffer, 22, checksum);

                int acount = 0;// writeHeader ? 0 : header_bytes + segments;

                for (int i = acount; i < count; i++)
                {
                    instream.WriteByte(page_buffer[i]);
                }
                seqno++;
                first = false;
                continued = next_continued;
                payload_bytes = 0;
            }

        }
    }

    //internal class ogStream3 : ogStream
    //{

    //    public ogStream3(Stream astream)
    //        : base(astream)
    //    {

    //    }

    //    public override byte[] getBytes()
    //    {
    //        flush_page();
    //        byte[] result = new byte[instream.Length];

    //        instream.Position = 0;
    //        instream.Read(result, 0, result.Length);
    //        return result;
    //    }

    //    protected override void flush_bits()
    //    {
    //        base.flush_bits();
    //    }

    //    //public override void flush_page(bool next_continued = false, bool last = false)
    //    //{
 
    //    //    flush_bits();
    //    //    int count = payload_bytes;
    //    //    for (int i = 0; i < count; i++)
    //    //    {
    //    //        instream.WriteByte(page_buffer[i]);
    //    //    }
    //    //    payload_bytes = 0;
    //    //}
    //}

    //internal class WwiseToVorbis : WwiseToOgg
    //{
    //    public WwiseToVorbis(Stream inStream, Stream outStream)
    //        : base(inStream, outStream)
    //    {

    //    }

    //    StreamState os;
    //    DspState vd;
    //    Block vb;
    //    public override bool ConvertToOgg(bool full_setup = false, bool _revorb = true)
    //    {
    //        os = new StreamState();
    //        Info vi = new Info();
    //        vi.init();
    //        vi.rate = Convert.ToInt32(_sample_rate);
    //        vi.channels = this._channels;
    //        vd = new DspState();
    //        vb = new Block(vd);


    //        return false;
    //    }

    //}


    //internal class ogStream2 : ogStream
    //{
    //    public ogStream2(Stream astream)
    //        : base(astream)
    //    {

    //    }



    //    public override void flush_page(bool next_continued = false, bool last = false)
    //    {
    //        if (payload_bytes != segment_size * max_segments)
    //        {
    //            flush_bits();
    //        }
    //        if (payload_bytes != 0)
    //        {
    //            byte[] packetbytes = new byte[payload_bytes];
    //            for (int i = 0; i < payload_bytes; i++)
    //            {
    //                packetbytes[i] = page_buffer[header_bytes + max_segments + i];
    //            }



    //            Ogg.Packet p = new Ogg.Packet();
    //            p.bytes = payload_bytes;
    //            p.packet = 0;
    //            p.packet_base = packetbytes;
    //            p.b_o_s = first ? 1 : 0;
    //            p.e_o_s = last ? 1 : 0;
    //            p.packetno = seqno;

    //            SyncState si = new SyncState();
    //            si.init();
    //            // StreamState stream_in = new StreamState();
    //            StreamState os = new StreamState();
    //            os.init(666);
    //            Info vi = new Info();
    //            vi.init();

    //            Page page = new Page();


    //            if (os.packetin(p) == 0)
    //            {
    //                while (os.flush(page) != 0)
    //                {
    //                    var size = instream.Length;
    //                    instream.Write(page.header_base, page.header, page.header_len);
    //                    instream.Write(page.body_base, page.body, page.body_len);

    //                    if (instream.Length != size + page.header_len + page.body_len)
    //                    {
    //                        //  fprintf(stderr,"Cannot write headers to output.\n");

    //                        os.clear();
    //                        throw new WwiseToOggException("os flush");
    //                    }
    //                    Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //                    //total += page.header_len + page.body_len;
    //                }

    //            }


    //            // var x = si.buffer(count);
    //            // Array.Copy(page_buffer, si.data, count);
    //            // si.wrote(count);

    //            // Page page = new Page();
    //            // if (si.pageout(page) != 1)
    //            // {
    //            //     throw new WwiseToOggException("error page out");
    //            // }


    //            // os.pagein(page);

    //            // while (os.flush(page) != 0)
    //            // {
    //            //     var size = instream.Length;
    //            //     instream.Write(page.header_base, page.header, page.header_len);
    //            //     instream.Write(page.body_base, page.body, page.body_len);

    //            //     if (instream.Length != size + page.header_len + page.body_len)
    //            //     {
    //            //         //  fprintf(stderr,"Cannot write headers to output.\n");

    //            //         os.clear();
    //            //         throw new WwiseToOggException("os flush");
    //            //     }
    //            //     Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //            //     //total += page.header_len + page.body_len;
    //            // }


    //            //int segments = ((payload_bytes + segment_size) / segment_size);  // intentionally round up
    //            //if (segments == max_segments + 1) segments = max_segments; // at max eschews the final 0
    //            ////ASegments = 0;
    //            //// move payload back
    //            //for (int i = 0; i < payload_bytes; i++)
    //            //{
    //            //    page_buffer[header_bytes + segments + i] =
    //            //        page_buffer[header_bytes + max_segments + i];
    //            //}

    //            //page_buffer[0] = Convert.ToByte('O');
    //            //page_buffer[1] = Convert.ToByte('g');
    //            //page_buffer[2] = Convert.ToByte('g');
    //            //page_buffer[3] = Convert.ToByte('S');
    //            //page_buffer[4] = 0; // stream_structure_version
    //            //page_buffer[5] = Convert.ToByte((continued ? 1 : 0) | (first ? 2 : 0) | (last ? 4 : 0));
    //            //page_buffer.write_64_le(6, granule);
    //            //page_buffer.write_32(14, 666);       // stream serial number
    //            //page_buffer.write_32(18, seqno);   // page sequence number
    //            //page_buffer.write_32(22, 0);       // checksum (0 for now)
    //            //page_buffer[26] = Convert.ToByte(segments);             // segment count
    //            //var bytes_left = payload_bytes;


    //            //int vals = 0;
    //            //byte[] lacing_vals = new byte[segments];
    //            //for (int i = 0; i < segments; i++)
    //            //{
    //            //    if (bytes_left >= segment_size)
    //            //    {
    //            //        bytes_left -= segment_size;
    //            //        lacing_vals[i] = segment_size;
    //            //    }
    //            //    else
    //            //    {
    //            //        lacing_vals[i] = Convert.ToByte(bytes_left);
    //            //    }
    //            //}


    //            //if (first)//b_o_s
    //            //{

    //            //} else
    //            //{




    //            //}


    //            //// lacing values
    //            //for (int i = 0; i < segments; i++)
    //            //{
    //            //    if (bytes_left >= segment_size)
    //            //    {
    //            //        bytes_left -= segment_size;
    //            //        page_buffer[27 + i] = segment_size;
    //            //    }
    //            //    else
    //            //    {
    //            //        page_buffer[27 + i] = Convert.ToByte(bytes_left);
    //            //    }
    //            //}

    //            //int count = header_bytes + segments + payload_bytes;

    //            //var checksum = CRC32.checksum(page_buffer, count);

    //            //MyBitWriter.write_32(page_buffer, 22, checksum);



    //            //for (int i = 0; i < header_bytes + segments + payload_bytes; i++)
    //            //{
    //            //    instream.WriteByte(page_buffer[i]);
    //            //}
    //            seqno++;
    //            first = false;
    //            continued = next_continued;
    //            payload_bytes = 0;
    //        }
    //    }

    //}


    /* Modern 6 byte header */
    internal class Packet6
    {
        Int32 _offset;
        Int16 _size;
        Int32 _absolute_granule;
        bool _no_granule;

        public Packet6(Stream i, Int32 offs, bool little_endian, bool no_gran = false)
        {
            _offset = offs;
            _size = -1;
            _absolute_granule = 0;
            _no_granule = no_gran;

            i.Seek(offs, SeekOrigin.Begin);

            if (little_endian)
            {
                _size = Convert.ToInt16(MyBitWriter.read_16_le(i));
                if (!no_gran)
                    _absolute_granule = Convert.ToInt32(MyBitWriter.read_32_le(i));
            }
            else
            {
                _size = Convert.ToInt16(MyBitWriter.read_16_be(i));
                if (!no_gran)
                    _absolute_granule = Convert.ToInt32(MyBitWriter.read_32_be(i));
            }
        }

        internal Int32 HeaderSize
        {
            get { return _no_granule ? 2 : 6; }
        }

        internal Int32 Offset
        {
            get { return _offset + HeaderSize; }
        }

        internal Int16 size { get { return _size; } }
        internal Int32 granule { get { return _absolute_granule; } }


        internal Int32 NextOffset
        {
            get { return _offset + HeaderSize + _size; }
        }
    }


    /* Old 8 byte header */
    internal class Packet8
    {
        Int32 _offset;
        Int32 _size;
        Int32 _absolute_granule;

        public Packet8(Stream i, Int32 o, bool little_endian)
        {
            _offset = o;
            _size = -1;
            _absolute_granule = 0;
            i.Seek(o, SeekOrigin.Begin);

            EndianBitConverter bc = LittleEndianBitConverter.Little;
            if (!little_endian)
                bc = BigEndianBitConverter.Big;

            EndianBinaryReader br = new EndianBinaryReader(bc, i);
            _size = br.ReadInt32();
            _absolute_granule = br.ReadInt32();
        }

        internal Int32 HeaderSize
        {
            get { return 8; }
        }

        internal Int32 Offset
        {
            get { return _offset + HeaderSize; }
        }

        internal Int32 size { get { return _size; } }
        internal Int32 granule { get { return _absolute_granule; } }

        internal Int32 NextOffset
        {
            get { return _offset + HeaderSize + _size; }
        }

    }


    internal class Vorbis_packet_header
    {
        uint _type;
        public byte[] vorbis_str { get; private set; }


        public Vorbis_packet_header(uint atype)
        {
            _type = atype;
            vorbis_str = new byte[6];
            vorbis_str[0] = Convert.ToByte('v');
            vorbis_str[1] = Convert.ToByte('o');
            vorbis_str[2] = Convert.ToByte('r');
            vorbis_str[3] = Convert.ToByte('b');
            vorbis_str[4] = Convert.ToByte('i');
            vorbis_str[5] = Convert.ToByte('s');
        }

        public uint ilength { get { return _type; } }

        //public void LeftShift(ogStream os)
        //{
        //    Bit_uint b = new Bit_uint(8, _type);
        //    b.LeftShift(os);

        //    for (int i = 0; i < 6; i++)
        //    {
        //        b = new Bit_uint(8, Convert.ToUInt32(vorbis_str[i]));
        //        b.LeftShift(os);
        //    }
        //}
    }

    internal class _bitStream
    {
        protected readonly Stream instream;
        protected int bit_buffer;
        protected int bits_left;
        protected int total_bits_read;

        public Stream baseStream { get { return instream; } }

        public _bitStream(Stream astream)
        {
            instream = astream;
            bit_buffer = 0;
            bits_left = 0;
            total_bits_read = 0;
        }
    }

    internal class bitStream : _bitStream
    {
        public bitStream(Stream astream)
            : base(astream)
        {
        }
        public bool get_bit()
        {
            if (bits_left == 0)
            {
                bit_buffer = instream.ReadByte();
                bits_left = 8;
            }

            total_bits_read++;
            bits_left--;
            return ((bit_buffer & (0x80 >> bits_left)) != 0);
        }

        public void Read(Bit_uint b)
        {
            b.ReadStream(this);
        }

        public Bit_uint Read(UInt32 bitsize)
        {
            var result = new Bit_uint(bitsize);
            Read(result);
            return result;
        }

        public int get_total_bits_read()
        {
            return total_bits_read;
        }

    }

    internal class Bit_uint
    {
        public UInt32 total { get; private set; }
        UInt32 m_BIT_SIZE = 0;

        public int totalI { get { return Convert.ToInt32(total); } }

        public uint bitsize { get { return m_BIT_SIZE; } }

        public Bit_uint(UInt32 size, UInt32 Total = 0)
        {
            total = Total;
            m_BIT_SIZE = size;
        }

        public Bit_uint(UInt32 size, Int32 Total)
        {
            total = Convert.ToUInt32(Total);
            m_BIT_SIZE = size;
        }

        public void ReadStream(bitStream AStream)
        {
            total = 0;
            for (int i = 0; i < m_BIT_SIZE; i++)
            {
                if (AStream.get_bit())
                    total |= 1U << i;
            }
        }

        public void Write(csBuffer buffer)
        {
            buffer.write(total, Convert.ToInt32(m_BIT_SIZE));
        }

        //public void LeftShift(ogStream AStream)
        //{
        //    for (int i = 0; i < BIT_SIZE; i++)
        //    {
        //        AStream.put_bit((total & (1U << i)) != 0);
        //    }
        //}

        public static implicit operator UInt32(Bit_uint x)
        {
            return x.total;
        }

    }


    //internal static class cBuffHelper
    //{
    //    public static void Write(this csBuffer buff, Bit_uint b)
    //    {
    //        b.Write(buff);
    //    }

    //    public static void Write(this csBuffer buff, Vorbis_packet_header v)
    //    {
    //        Write(buff, new Bit_uint(8, v.ilength));
    //        for (int i = 0; i < 6; i++)
    //            Write(buff, new Bit_uint(8, Convert.ToUInt32(v.vorbis_str[i])));

    //    }

    //    /*  public void write(Vorbis_packet_header v)
    //    {
    //        write(new Bit_uint(8, v.ilength));
    //        for (int i = 0; i < 6; i++)
    //            write(new Bit_uint(8, Convert.ToUInt32(v.vorbis_str[i])));
    //    }*/

    //    //Vorbis_packet_header
    //}

    //internal class WwiseToOgg3 : WwiseToOgg
    //{
    //    private static string _vorbis = "vorbis";
    //    public WwiseToOgg3(Stream inStream, Stream outStream)
    //        : base(inStream, outStream)
    //    {

    //    }

    //    public bool genSetup(csBuffer os)
    //    {
    //        os.Write(new Vorbis_packet_header(5));
    //        //  vhead.LeftShift(os);

    //        Packet6 setup_packet = new Packet6(FInStream, _data_offset + _setup_packet_offset, _little_endian, _no_granule);

    //        FInStream.Seek(setup_packet.Offset, SeekOrigin.Begin);

    //        if (setup_packet.granule != 0)
    //            throw new WwiseToOggException("setup packet granule != 0");

    //        bitStream ss = new bitStream(FInStream);
    //        Bit_uint codebook_count_less1 = ss.Read(8);

    //        UInt32 codebook_count = codebook_count_less1 + 1;
    //        os.Write(codebook_count_less1);

    //        if (_inline_codebooks)
    //        {
    //            codebook_library cbl = new codebook_library();

    //            for (int i = 0; i < codebook_count; i++)
    //            {
    //                if (_full_setup)
    //                {
    //                    // cbl.copy(ss, os);
    //                }
    //                else
    //                {
    //                    cbl.rebuild(ss, 0, os);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            byte[] data = null; //AudioTools.Properties.Resources.packed_codebooks_aoTuV_603;
    //            //byte[] data = AudioTools.Properties.Resources.packed_codebooks;


    //            if (String.IsNullOrEmpty(FCodebooksName))
    //                FCodebooksName = "packed_codebooks_aoTuV_603";

    //            if (File.Exists(FCodebooksName))
    //            {
    //                using (FileStream fcn = File.OpenRead(FCodebooksName))
    //                {
    //                    data = new byte[fcn.Length];
    //                    fcn.Read(data, 0, data.Length);
    //                }

    //            }
    //            else
    //            {
    //                data = (byte[])AudioTools.Properties.Resources.ResourceManager.GetObject(FCodebooksName);
    //            }
    //            if (data == null)
    //                data = AudioTools.Properties.Resources.packed_codebooks_aoTuV_603;



    //            using (MemoryStream ms = new MemoryStream(data))
    //            {
    //                ms.Position = 0;
    //                codebook_library cbl = new codebook_library(ms);
    //                ms.Position = 0;
    //                for (int i = 0; i < codebook_count; i++)
    //                {
    //                    Bit_uint codebook_id = new Bit_uint(10);
    //                    ss.Read(codebook_id);

    //                    try
    //                    {
    //                        cbl.rebuild(Convert.ToInt32(codebook_id), os);
    //                    }
    //                    catch (codebook_library.Invalid_id E)
    //                    {

    //                        if (codebook_id == 0x342)
    //                        {
    //                            var codebook_identifier = ss.Read(14);

    //                            //         B         C         V
    //                            //    4    2    4    3    5    6
    //                            // 0100 0010 0100 0011 0101 0110
    //                            //           \_____|_ _|_______/
    //                            //                   X
    //                            //         01 0101 10 01 0000
    //                            if (codebook_identifier == 0x1590)
    //                            {
    //                                // starts with BCV, probably --full-setup
    //                                throw new WwiseToOggException(
    //                                    "invalid codebook id 0x342, try --full-setup");
    //                            }
    //                        }

    //                        // just an invalid codebook
    //                        throw E;


    //                    }
    //                }
    //            }
    //        }

    //        Bit_uint time_count_less1 = new Bit_uint(6);
    //        os.Write(time_count_less1);
    //        Bit_uint dummy_time_value = new Bit_uint(16);
    //        os.Write(dummy_time_value);

    //        if (_full_setup)
    //        {

    //            while (ss.get_total_bits_read() < setup_packet.size * 8u)
    //            {
    //                os.Write(ss.Read(1));
    //            }
    //        }
    //        else    // _full_setup
    //        {
    //            //alot of fucking code...
    //            // Always floor type 1


    //            var floor_count_less1 = ss.Read(6);
    //            uint floor_count = floor_count_less1 + 1;
    //            os.Write(floor_count_less1);

    //            // rebuild floors
    //            for (int i = 0; i < floor_count; i++)
    //            {
    //                os.Write(new Bit_uint(16, 1));
    //                var floor1_part = ss.Read(5);
    //                os.Write(floor1_part);

    //                uint[] floor1_partition_class_list = new uint[floor1_part];
    //                uint minclass = 0;
    //                for (int j = 0; j < floor1_part; j++)
    //                {
    //                    var floor1_partition_class = ss.Read(4);
    //                    os.Write(floor1_partition_class);

    //                    floor1_partition_class_list[j] = floor1_partition_class;

    //                    if (floor1_partition_class > minclass)
    //                        minclass = floor1_partition_class;
    //                }

    //                uint[] floor1_class_dimensions_list = new uint[minclass + 1];

    //                for (int j = 0; j <= minclass; j++)
    //                {
    //                    var class_dimensions_less1 = ss.Read(3);
    //                    os.Write(class_dimensions_less1);

    //                    floor1_class_dimensions_list[j] = class_dimensions_less1 + 1;

    //                    var class_subclasses = ss.Read(2);
    //                    os.Write(class_subclasses);

    //                    if (class_subclasses != 0)
    //                    {
    //                        var masterbook = ss.Read(8);
    //                        os.Write(masterbook);

    //                        if (masterbook >= codebook_count)
    //                            throw new WwiseToOggException("invalid floor1 masterbook");
    //                    }

    //                    for (int k = 0; k < (1U << class_subclasses.totalI); k++)
    //                    {
    //                        var subclass_book_plus1 = ss.Read(8);
    //                        os.Write(subclass_book_plus1);

    //                        int subclass_book = Convert.ToInt32(subclass_book_plus1) - 1;
    //                        if (subclass_book >= 0 && Convert.ToUInt32(subclass_book) >= codebook_count)
    //                            throw new WwiseToOggException("invalid floor1 subclass book");
    //                    }
    //                }
    //                os.Write(ss.Read(2));//floor1_multiplier_less1
    //                var rangebits = ss.Read(4);
    //                os.Write(rangebits);
    //                uint rb = rangebits;

    //                for (int j = 0; j < floor1_part; j++)
    //                {
    //                    var current_class_number = floor1_partition_class_list[j];
    //                    for (int k = 0; k < floor1_class_dimensions_list[current_class_number]; k++)
    //                    {
    //                        os.Write(ss.Read(rb));
    //                    }
    //                }
    //            }
    //            var residue_count_less1 = ss.Read(6);
    //            var residue_count = residue_count_less1 + 1;
    //            os.Write(residue_count_less1);

    //            for (int i = 0; i < residue_count; i++)
    //            {
    //                var residue_type = ss.Read(2);
    //                os.Write(new Bit_uint(16, residue_type));
    //                if (residue_type > 2)
    //                    throw new WwiseToOggException("invalid residue type");

    //                var residue_begin = ss.Read(24);
    //                var residue_end = ss.Read(24);
    //                var residue_partition_size_less1 = ss.Read(24);
    //                var residue_classifications_less1 = ss.Read(6);
    //                var residue_classbook = ss.Read(8);

    //                uint residue_classifications = residue_classifications_less1 + 1;

    //                os.Write(residue_begin);
    //                os.Write(residue_end);
    //                os.Write(residue_partition_size_less1);
    //                os.Write(residue_classifications_less1);
    //                os.Write(residue_classbook);
    //                if (residue_classbook >= codebook_count) throw new WwiseToOggException("invalid residue classbook");

    //                uint[] residue_cascade = new uint[residue_classifications];

    //                for (int j = 0; j < residue_classifications; j++)
    //                {
    //                    var high_bits = new Bit_uint(5);
    //                    var low_bits = ss.Read(3);
    //                    os.Write(low_bits);

    //                    var bitflag = ss.Read(1);
    //                    os.Write(bitflag);
    //                    if (bitflag != 0)
    //                    {
    //                        ss.Read(high_bits);
    //                        os.Write(high_bits);
    //                    }

    //                    residue_cascade[j] = high_bits * 8 + low_bits;
    //                }

    //                for (int j = 0; j < residue_classifications; j++)
    //                {
    //                    for (int k = 0; k < 8; k++)
    //                    {
    //                        if ((Convert.ToInt32(residue_cascade[j]) & (1 << k)) != 0)
    //                        {
    //                            var residue_book = ss.Read(8);
    //                            os.Write(residue_book);

    //                            if (residue_book >= codebook_count) throw new WwiseToOggException("invalid residue book");
    //                        }
    //                    }
    //                }
    //            }


    //            // mapping count
    //            var mapping_count_less1 = ss.Read(6);
    //            uint mapping_count = mapping_count_less1 + 1;
    //            os.Write(mapping_count_less1);

    //            for (int i = 0; i < mapping_count; i++)
    //            {
    //                // always mapping type 0, the only one
    //                //var mapping_type = new Bit_uint(16);

    //                os.Write(new Bit_uint(16));//mapping_type);

    //                var submaps_flag = ss.Read(1);
    //                os.Write(submaps_flag);

    //                uint submaps = 1;
    //                if (submaps_flag != 0)
    //                {
    //                    var submaps_less1 = ss.Read(4);
    //                    submaps = submaps_less1 + 1;
    //                    os.Write(submaps_less1);
    //                }

    //                var square_polar_flag = ss.Read(1);
    //                os.Write(square_polar_flag);

    //                if (square_polar_flag != 0)
    //                {
    //                    var coupling_steps_less1 = ss.Read(8);
    //                    uint coupling_steps = coupling_steps_less1 + 1;
    //                    os.Write(coupling_steps_less1);

    //                    for (int j = 0; j < coupling_steps; j++)
    //                    {
    //                        var magnitude = ss.Read(Convert.ToUInt32(_channels - 1));
    //                        var angle = ss.Read(Convert.ToUInt32(_channels - 1));
    //                        os.Write(magnitude);
    //                        os.Write(angle);

    //                        if (angle == magnitude || magnitude >= _channels || angle >= _channels) throw new WwiseToOggException("invalid coupling");
    //                    }
    //                }

    //                // a rare reserved field not removed by Ak!
    //                var mapping_reserved = ss.Read(2);
    //                os.Write(mapping_reserved);
    //                if (0 != mapping_reserved) throw new WwiseToOggException("mapping reserved field nonzero");

    //                if (submaps > 1)
    //                {
    //                    for (int j = 0; j < _channels; j++)
    //                    {
    //                        var mapping_mux = ss.Read(4);
    //                        os.Write(mapping_mux);

    //                        if (mapping_mux >= submaps) throw new WwiseToOggException("mapping_mux >= submaps");
    //                    }
    //                }

    //                for (int j = 0; j < submaps; j++)
    //                {
    //                    // Another! Unused time domain transform configuration placeholder!
    //                    var time_config = ss.Read(8);
    //                    os.Write(time_config);

    //                    var floor_number = ss.Read(8);
    //                    os.Write(floor_number);
    //                    if (floor_number >= floor_count) throw new WwiseToOggException("invalid floor mapping");

    //                    var residue_number = ss.Read(8);
    //                    os.Write(residue_number);
    //                    if (residue_number >= residue_count) throw new WwiseToOggException("invalid residue mapping");
    //                }
    //            }



    //            // mode count
    //            var mode_count_less1 = ss.Read(6);
    //            uint mode_count = mode_count_less1 + 1;
    //            os.Write(mode_count_less1);
    //            //     mode_blockflag.Clear();
    //            //mode_blockflag = new bool[mode_count];
    //            mode_bits = (mode_count - 1);

    //            //cout << mode_count << " modes" << endl;

    //            for (int i = 0; i < mode_count; i++)
    //            {
    //                var block_flag = ss.Read(1);
    //                os.Write(block_flag);

    //                // mode_blockflag.Add(block_flag != 0);

    //                // only 0 valid for windowtype and transformtype
    //                var windowtype = new Bit_uint(16);
    //                var transformtype = new Bit_uint(16);

    //                os.Write(windowtype);
    //                os.Write(transformtype);

    //                var mapping = ss.Read(8);
    //                os.Write(mapping);
    //                if (mapping >= mapping_count) throw new WwiseToOggException("invalid mode mapping");
    //            }
    //            os.Write(new Bit_uint(1, 1));//framing
    //            // Bit_uint<1> framing(1);
    //            //  os << framing;

    //        }
    //        // os.flush_page();
    //        if ((ss.get_total_bits_read() + 7) / 8 != setup_packet.size) throw new WwiseToOggException("didn't read exactly setup packet");

    //        if (setup_packet.NextOffset != _data_offset + _first_audio_packet_offset) throw new WwiseToOggException("first audio packet doesn't follow setup packet");

    //        return true;
    //    }

    //    public void CreateHeaders()
    //    {
    //        csBuffer buff = new csBuffer();
    //        buff.writeinit();
    //        Encoding AE = Encoding.UTF8;
    //        byte[] _vorbis_byt = AE.GetBytes(_vorbis);

    //        //// preamble
    //        buff.write(0x01, 8);
    //        buff.write(_vorbis_byt);

    //        //// basic information about the stream
    //        buff.write(0x00, 32);
    //        buff.write(_channels, 8);
    //        buff.write(Convert.ToInt32(_sample_rate), 32);

    //        buff.write(0, 32);
    //        buff.write(Convert.ToInt32(_avg_bytes_per_second * 8), 32);
    //        buff.write(0, 32);
    //        buff.write(_blocksize_0_pow, 4);
    //        buff.write(_blocksize_1_pow, 4);
    //        buff.write(1, 1);



    //        Packet packetX = new Packet()
    //        {
    //            packet_base = buff.buf(),
    //            packet = 0,
    //            bytes = buff.bytes(),
    //            b_o_s = 1,
    //            e_o_s = 0,
    //            packetno = 0
    //        };
    //        StreamState os = new StreamState();

    //        if (os.packetin(packetX) == 0)
    //        {
    //            Page page = new Page();
    //            while (os.flush(page) != 0)
    //            {
    //                var size = FOutStream.Length;
    //                FOutStream.Write(page.header_base, page.header, page.header_len);
    //                FOutStream.Write(page.body_base, page.body, page.body_len);

    //                if (FOutStream.Length != size + page.header_len + page.body_len)
    //                {
    //                    os.clear();
    //                    throw new WwiseToOggException("os flush");
    //                }
    //                Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //            }
    //        }

    //        buff.writeclear();
    //        buff.writeinit();


    //        buff.write(0x03, 8);
    //        buff.write(_vorbis_byt);

    //        // const string vendor = "converted from Audiokinetic Wwise by df_oggconvert v1.0 ";
    //        const string vendor = "converted from Wwise by dfaudiolib v0.8";
    //        Bit_uint vendorsize = new Bit_uint(32, Convert.ToUInt32(vendor.Length));
    //        vendorsize.Write(buff);

    //        for (int i = 0; i < vendorsize; i++)
    //        {
    //            byte b = Convert.ToByte(vendor[i]);
    //            new Bit_uint(8, Convert.ToUInt32(b)).Write(buff);
    //        }

    //        if (_loop_count == 0)
    //        {
    //            new Bit_uint(32, 0).Write(buff);
    //        }
    //        else
    //        {
    //            Bit_uint user_comment_count = new Bit_uint(32, 2);
    //            user_comment_count.Write(buff);

    //            string loop_start_str = "LoopStart=" + _loop_start;
    //            string loop_end_str = "LoopEnd=" + _loop_end;

    //            Bit_uint loop_start_comment_length = new Bit_uint(32, loop_start_str.Length);
    //            loop_start_comment_length.Write(buff);

    //            for (int i = 0; i < loop_start_comment_length; i++)
    //            {
    //                byte b = Convert.ToByte(loop_start_str[i]);
    //                Bit_uint c = new Bit_uint(8, Convert.ToUInt32(b));
    //                c.Write(buff);
    //            }

    //            Bit_uint loop_end_comment_length = new Bit_uint(32, loop_end_str.Length);
    //            loop_end_comment_length.Write(buff);

    //            for (int i = 0; i < loop_end_comment_length; i++)
    //            {
    //                byte b = Convert.ToByte(loop_end_str[i]);
    //                Bit_uint c = new Bit_uint(8, Convert.ToUInt32(b));
    //                c.Write(buff);
    //            }
    //        }

    //        Bit_uint framing = new Bit_uint(1, 1);
    //        framing.Write(buff);

    //        genSetup(buff);

    //        packetX = new Packet()
    //        {
    //            packet_base = buff.buf(),
    //            packet = 0,
    //            bytes = buff.bytes(),
    //            b_o_s = 0,
    //            e_o_s = 0,
    //            packetno = 0
    //        };

    //        if (os.packetin(packetX) == 0)
    //        {
    //            Page page = new Page();
    //            while (os.flush(page) != 0)
    //            {
    //                var size = FOutStream.Length;
    //                FOutStream.Write(page.header_base, page.header, page.header_len);
    //                FOutStream.Write(page.body_base, page.body, page.body_len);

    //                if (FOutStream.Length != size + page.header_len + page.body_len)
    //                {
    //                    os.clear();
    //                    throw new WwiseToOggException("os flush");
    //                }
    //                Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //            }
    //        }

    //        //buff.write(ilog2(blocksizes[0]), 4);
    //        //buff.write(ilog2(blocksizes[1]), 4);
    //        //buff.write(1, 1);

    //    }

    //    public override bool ConvertToOgg(bool full_setup = false, bool _revorb = true)
    //    {
    //        CreateHeaders();
    //        FOutStream.Position = 0;
    //        FOutStream.CopyTo(FRealOutStream);

    //        return false;
    //    }

    //}
    //internal class WwiseToOgg2 : WwiseToOgg
    //{
    //    const int serial = 666;

    //    public WwiseToOgg2(Stream inStream, Stream outStream)
    //        : base(inStream, outStream)
    //    {

    //    }

    //    private bool writevorbheader(int size, StreamWriter sw)
    //    {
    //        sw.Write(size);
    //        byte[] b = Encoding.ASCII.GetBytes("vorbis");
    //        sw.Write(b);
    //        return true;
    //    }

    //    private byte[] createVorbBody(int size, byte[] xdata,int length = 0)
    //    {
    //        if (length == 0)
    //            length = xdata.Length;

    //        byte[] b = Encoding.ASCII.GetBytes("vorbis");
    //        byte[] result = new byte[b.Length + 4 + length];
    //        byte[] asize = BitConverter.GetBytes(size);
    //        Array.Copy(asize, result, 4);
    //        Array.Copy(b, 0, result, 4, b.Length);
    //        Array.Copy(xdata, 0, result, 4 + b.Length, length);
    //        return result;
    //    }

    //    public override bool ConvertToOgg(bool full_setup = false, bool _revorb = true)
    //    {
    //        SyncState sync_in = new SyncState();
    //        sync_in.init();
    //        StreamState stream_in = new StreamState();
    //        StreamState stream_out = new StreamState();
    //        bool g_failed = true;
    //        Info vi = new Info();
    //        vi.init();

    //        Packet packet = new Packet();
    //        Page page = new Page();

    //        if (create_headers(sync_in, stream_in, stream_out, vi))
    //        {

    //        }
    //        FOutStream.Position = 0;
    //        FOutStream.CopyTo(FRealOutStream);

    //        return !g_failed;// base.ConvertToOgg(full_setup, _revorb);
    //    }

    //    private bool create_headers(SyncState si, StreamState iss, StreamState os, Info vi)
    //    {
    //        //StreamState ss = new StreamState();
    //        //ss.init(serial);

    //        csBuffer opb = new csBuffer();
    //        Encoding AE = Encoding.UTF8;
    //        byte[] _vorbis_byt = AE.GetBytes("vorbis");
    //        opb.writeinit();
    //        // preamble
    //        opb.write(0x01, 8);
    //        opb.write(_vorbis_byt);

    //        // basic information about the stream
    //        opb.write(0x00, 32);
    //        opb.write(_channels, 8);
    //        opb.write(Convert.ToInt32(_sample_rate), 32);

    //        opb.write(0, 32);
    //        opb.write(Convert.ToInt32(_avg_bytes_per_second * 8), 32);
    //        opb.write(0, 32);

    //        opb.write(_blocksize_0_pow, 4);
    //        opb.write(_blocksize_1_pow, 4);
    //        opb.write(1, 1);



    //        Packet packetX = new Packet();
    //        packetX.packet_base = opb.buf();
    //        packetX.packet = 0;
    //        packetX.bytes = opb.bytes();
    //        packetX.b_o_s = 1;
    //        packetX.e_o_s = 0;
    //        packetX.packetno = 0;

    //        if (os.packetin(packetX) == 0)
    //        {
    //            Page page = new Page();
    //            while (os.flush(page) != 0)
    //            {
    //                var size = FOutStream.Length;
    //                FOutStream.Write(page.header_base, page.header, page.header_len);
    //                FOutStream.Write(page.body_base, page.body, page.body_len);

    //                if (FOutStream.Length != size + page.header_len + page.body_len)
    //                {
    //                    os.clear();
    //                    throw new WwiseToOggException("os flush");
    //                }
    //                Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //            }
    //        }

    //        csBuffer osb = new csBuffer();
    //        osb.writeinit();
    //        osb.Write(new Vorbis_packet_header(3));

    //        // const string vendor = "converted from Audiokinetic Wwise by df_oggconvert v1.0 ";
    //        const string vendor = "converted from Wwise by dfaudiolib v0.8";
    //        Bit_uint vendorsize = new Bit_uint(32, Convert.ToUInt32(vendor.Length));
    //        osb.Write(vendorsize);

    //        for (int i = 0; i < vendorsize; i++)
    //            osb.Write(new Bit_uint(8, Convert.ToUInt32(Convert.ToByte(vendor[i]))));

    //        if (_loop_count == 0)
    //            osb.Write(new Bit_uint(32, 0));
    //        else
    //        {
    //            osb.Write(new Bit_uint(32, 2));//user_comment_count

    //            string loop_start_str = "LoopStart=" + _loop_start;
    //            string loop_end_str = "LoopEnd=" + _loop_end;

    //            Bit_uint loop_start_comment_length = new Bit_uint(32, loop_start_str.Length);
    //            osb.Write(loop_start_comment_length);

    //            for (int i = 0; i < loop_start_comment_length; i++)
    //                osb.Write(new Bit_uint(8, Convert.ToUInt32(Convert.ToByte(loop_start_str[i]))));

    //            Bit_uint loop_end_comment_length = new Bit_uint(32, loop_end_str.Length);
    //            osb.Write(loop_end_comment_length);

    //            for (int i = 0; i < loop_end_comment_length; i++)
    //            {
    //                osb.Write(new Bit_uint(8, Convert.ToUInt32(Convert.ToByte(loop_end_str[i]))));
    //            }
    //        }
    //        osb.Write(new Bit_uint(1, 1));//framing


    //        //os.FLUSH_BITS();
    //        Packet p = new Packet() { packet_base = osb.buf(), bytes = osb.bytes(), b_o_s = 0, e_o_s = 0 };

    //        Comment vc = new Comment();
    //        vc.init();

    //        vi.rate = Convert.ToInt32(_sample_rate);
    //        vi.channels = Convert.ToInt32(_channels);
    //       // vc.add("converted from Wwise by dfaudiolib v0.8");

    //        if (vi.synthesis_headerin(vc, p) < 0)
    //        {
    //            return false;
    //        }

    //        os.packetin(p);
    //        int packnum = 1;
    //        Packet6 setup_packet = new Packet6(FInStream, _data_offset + _setup_packet_offset, _little_endian, _no_granule);

    //        FInStream.Seek(setup_packet.Offset, SeekOrigin.Begin);
    //        osb.writeclear();
    //        osb.writeinit();


    //        if (setup_packet.granule != 0)
    //            throw new WwiseToOggException("setup packet granule != 0");
    //        //  MemoryStream ms = new MemoryStream();
    //        //  using (StreamWriter sw = new StreamWriter(ms))
    //        {
    //            bitStream ss1 = new bitStream(FInStream);
    //            Bit_uint codebook_count_less1 = ss1.Read(8);
    //            UInt32 codebook_count = codebook_count_less1 + 1;
    //            osb.writeUInt(codebook_count);
    //           // FOutStream.Write(BitConverter.GetBytes(codebook_count), 0, 4);
    //            //sw.Write(codebook_count);
    //            if (_inline_codebooks)
    //            {
    //                //fuck shit...
    //            }
    //            else
    //            {
    //                byte[] data = null; //AudioTools.Properties.Resources.packed_codebooks_aoTuV_603;
    //                //byte[] data = AudioTools.Properties.Resources.packed_codebooks;


    //                if (String.IsNullOrEmpty(FCodebooksName))
    //                    FCodebooksName = "packed_codebooks_aoTuV_603";

    //                if (File.Exists(FCodebooksName))
    //                {
    //                    using (FileStream fcn = File.OpenRead(FCodebooksName))
    //                    {
    //                        data = new byte[fcn.Length];
    //                        fcn.Read(data, 0, data.Length);
    //                    }

    //                }
    //                else
    //                {
    //                    data = (byte[])AudioTools.Properties.Resources.ResourceManager.GetObject(FCodebooksName);
    //                }
    //                if (data == null)
    //                    data = AudioTools.Properties.Resources.packed_codebooks_aoTuV_603;

    //                using (MemoryStream ms2 = new MemoryStream(data))
    //                {
    //                    ms2.Position = 0;
    //                    codebook_library cbl = new codebook_library(ms2);
    //                    // ms.Position = 0;
    //                 //   MemoryStream ms3 = new MemoryStream();
    //                 //   ogStream os1 = new ogStream3(ms3);
    //                    for (int i = 0; i < codebook_count; i++)
    //                    {
    //                        Bit_uint codebook_id = new Bit_uint(10);
    //                        ss1.Read(codebook_id);

    //                        try
    //                        {
    //                            cbl.rebuild(Convert.ToInt32(codebook_id), osb);
    //                        }
    //                        catch (codebook_library.Invalid_id E)
    //                        {

    //                            if (codebook_id == 0x342)
    //                            {
    //                                var codebook_identifier = ss1.Read(14);

    //                                //         B         C         V
    //                                //    4    2    4    3    5    6
    //                                // 0100 0010 0100 0011 0101 0110
    //                                //           \_____|_ _|_______/
    //                                //                   X
    //                                //         01 0101 10 01 0000
    //                                if (codebook_identifier == 0x1590)
    //                                {
    //                                    // starts with BCV, probably --full-setup
    //                                    throw new WwiseToOggException(
    //                                        "invalid codebook id 0x342, try --full-setup");
    //                                }
    //                            }

    //                            // just an invalid codebook
    //                            throw E;
    //                        }
    //                    }

    //                    //  var xbytes = os1.getBytes();

    //                   // os1.flush_page();


                        

    //                    packetX = new Packet();
    //                    packetX.packet_base = osb.buf();
    //                    packetX.packet = 0;
    //                    packetX.bytes = osb.bytes();
    //                    packetX.b_o_s = 0;
    //                    packetX.e_o_s = 0;
    //                    packetX.packetno = packnum++;
    //                    os.packetin(packetX);
    //                }
    //            }
    //            osb.writeclear();
    //            osb.writeinit();

    //           // using (MemoryStream ms3 = new MemoryStream())
    //            {
    //               // ogStream3 os3 = new ogStream3(ms3);

    //                Bit_uint time_count_less1 = new Bit_uint(6);
    //                osb.Write(time_count_less1);
    //                Bit_uint dummy_time_value = new Bit_uint(16);
    //                osb.Write(dummy_time_value);

    //                if (_full_setup)
    //                {

    //                    while (ss1.get_total_bits_read() < setup_packet.size * 8u)
    //                    {
    //                        osb.Write(ss1.Read(1));
    //                    }
    //                }
    //                else    // _full_setup
    //                {
    //                    //alot of fucking code...
    //                    // Always floor type 1


    //                    var floor_count_less1 = ss1.Read(6);
    //                    uint floor_count = floor_count_less1 + 1;
    //                    osb.Write(floor_count_less1);

    //                    // rebuild floors
    //                    for (int i = 0; i < floor_count; i++)
    //                    {
    //                        osb.Write(new Bit_uint(16, 1));
    //                        var floor1_part = ss1.Read(5);
    //                        osb.Write(floor1_part);

    //                        uint[] floor1_partition_class_list = new uint[floor1_part];
    //                        uint minclass = 0;
    //                        for (int j = 0; j < floor1_part; j++)
    //                        {
    //                            var floor1_partition_class = ss1.Read(4);
    //                            osb.Write(floor1_partition_class);

    //                            floor1_partition_class_list[j] = floor1_partition_class;

    //                            if (floor1_partition_class > minclass)
    //                                minclass = floor1_partition_class;
    //                        }

    //                        uint[] floor1_class_dimensions_list = new uint[minclass + 1];

    //                        for (int j = 0; j <= minclass; j++)
    //                        {
    //                            var class_dimensions_less1 = ss1.Read(3);
    //                            osb.Write(class_dimensions_less1);

    //                            floor1_class_dimensions_list[j] = class_dimensions_less1 + 1;

    //                            var class_subclasses = ss1.Read(2);
    //                            osb.Write(class_subclasses);

    //                            if (class_subclasses != 0)
    //                            {
    //                                var masterbook = ss1.Read(8);
    //                                osb.Write(masterbook);

    //                                if (masterbook >= codebook_count)
    //                                    throw new WwiseToOggException("invalid floor1 masterbook");
    //                            }

    //                            for (int k = 0; k < (1U << class_subclasses.totalI); k++)
    //                            {
    //                                var subclass_book_plus1 = ss1.Read(8);
    //                                osb.Write(subclass_book_plus1);

    //                                int subclass_book = Convert.ToInt32(subclass_book_plus1) - 1;
    //                                if (subclass_book >= 0 && Convert.ToUInt32(subclass_book) >= codebook_count)
    //                                    throw new WwiseToOggException("invalid floor1 subclass book");
    //                            }
    //                        }
    //                        osb.Write(ss1.Read(2));//floor1_multiplier_less1
    //                        var rangebits = ss1.Read(4);
    //                        osb.Write(rangebits);
    //                        uint rb = rangebits;

    //                        for (int j = 0; j < floor1_part; j++)
    //                        {
    //                            var current_class_number = floor1_partition_class_list[j];
    //                            for (int k = 0; k < floor1_class_dimensions_list[current_class_number]; k++)
    //                            {
    //                                osb.Write(ss1.Read(rb));
    //                            }
    //                        }
    //                    }
    //                    var residue_count_less1 = ss1.Read(6);
    //                    var residue_count = residue_count_less1 + 1;
    //                    osb.Write(residue_count_less1);

    //                    for (int i = 0; i < residue_count; i++)
    //                    {
    //                        var residue_type = ss1.Read(2);
    //                        osb.Write(new Bit_uint(16, residue_type));
    //                        if (residue_type > 2)
    //                            throw new WwiseToOggException("invalid residue type");

    //                        var residue_begin = ss1.Read(24);
    //                        var residue_end = ss1.Read(24);
    //                        var residue_partition_size_less1 = ss1.Read(24);
    //                        var residue_classifications_less1 = ss1.Read(6);
    //                        var residue_classbook = ss1.Read(8);

    //                        uint residue_classifications = residue_classifications_less1 + 1;

    //                        osb.Write(residue_begin);
    //                        osb.Write(residue_end);
    //                        osb.Write(residue_partition_size_less1);
    //                        osb.Write(residue_classifications_less1);
    //                        osb.Write(residue_classbook);
    //                        if (residue_classbook >= codebook_count) throw new WwiseToOggException("invalid residue classbook");

    //                        uint[] residue_cascade = new uint[residue_classifications];

    //                        for (int j = 0; j < residue_classifications; j++)
    //                        {
    //                            var high_bits = new Bit_uint(5);
    //                            var low_bits = ss1.Read(3);
    //                            osb.Write(low_bits);

    //                            var bitflag = ss1.Read(1);
    //                            osb.Write(bitflag);
    //                            if (bitflag != 0)
    //                            {
    //                                ss1.Read(high_bits);
    //                                osb.Write(high_bits);
    //                            }

    //                            residue_cascade[j] = high_bits * 8 + low_bits;
    //                        }

    //                        for (int j = 0; j < residue_classifications; j++)
    //                        {
    //                            for (int k = 0; k < 8; k++)
    //                            {
    //                                if ((Convert.ToInt32(residue_cascade[j]) & (1 << k)) != 0)
    //                                {
    //                                    var residue_book = ss1.Read(8);
    //                                    osb.Write(residue_book);

    //                                    if (residue_book >= codebook_count) throw new WwiseToOggException("invalid residue book");
    //                                }
    //                            }
    //                        }
    //                    }


    //                    // mapping count
    //                    var mapping_count_less1 = ss1.Read(6);
    //                    uint mapping_count = mapping_count_less1 + 1;
    //                    osb.Write(mapping_count_less1);

    //                    for (int i = 0; i < mapping_count; i++)
    //                    {
    //                        // always mapping type 0, the only one
    //                        //var mapping_type = new Bit_uint(16);

    //                        osb.Write(new Bit_uint(16));//mapping_type);

    //                        var submaps_flag = ss1.Read(1);
    //                        osb.Write(submaps_flag);

    //                        uint submaps = 1;
    //                        if (submaps_flag != 0)
    //                        {
    //                            var submaps_less1 = ss1.Read(4);
    //                            submaps = submaps_less1 + 1;
    //                            osb.Write(submaps_less1);
    //                        }

    //                        var square_polar_flag = ss1.Read(1);
    //                        osb.Write(square_polar_flag);

    //                        if (square_polar_flag != 0)
    //                        {
    //                            var coupling_steps_less1 = ss1.Read(8);
    //                            uint coupling_steps = coupling_steps_less1 + 1;
    //                            osb.Write(coupling_steps_less1);

    //                            for (int j = 0; j < coupling_steps; j++)
    //                            {
    //                                var magnitude = ss1.Read(Convert.ToUInt32(_channels - 1));
    //                                var angle = ss1.Read(Convert.ToUInt32(_channels - 1));
    //                                osb.Write(magnitude);
    //                                osb.Write(angle);

    //                                if (angle == magnitude || magnitude >= _channels || angle >= _channels) throw new WwiseToOggException("invalid coupling");
    //                            }
    //                        }

    //                        // a rare reserved field not removed by Ak!
    //                        var mapping_reserved = ss1.Read(2);
    //                        osb.Write(mapping_reserved);
    //                        if (0 != mapping_reserved) throw new WwiseToOggException("mapping reserved field nonzero");

    //                        if (submaps > 1)
    //                        {
    //                            for (int j = 0; j < _channels; j++)
    //                            {
    //                                var mapping_mux = ss1.Read(4);
    //                                osb.Write(mapping_mux);

    //                                if (mapping_mux >= submaps) throw new WwiseToOggException("mapping_mux >= submaps");
    //                            }
    //                        }

    //                        for (int j = 0; j < submaps; j++)
    //                        {
    //                            // Another! Unused time domain transform configuration placeholder!
    //                            var time_config = ss1.Read(8);
    //                            osb.Write(time_config);

    //                            var floor_number = ss1.Read(8);
    //                            osb.Write(floor_number);
    //                            if (floor_number >= floor_count) throw new WwiseToOggException("invalid floor mapping");

    //                            var residue_number = ss1.Read(8);
    //                            osb.Write(residue_number);
    //                            if (residue_number >= residue_count) throw new WwiseToOggException("invalid residue mapping");
    //                        }
    //                    }



    //                    // mode count
    //                    var mode_count_less1 = ss1.Read(6);
    //                    uint mode_count = mode_count_less1 + 1;
    //                    osb.Write(mode_count_less1);
    //                    //mode_blockflag.Clear();
    //                    //mode_blockflag = new bool[mode_count];
    //                    mode_bits = (mode_count - 1);

    //                    //cout << mode_count << " modes" << endl;

    //                    for (int i = 0; i < mode_count; i++)
    //                    {
    //                        var block_flag = ss1.Read(1);
    //                        osb.Write(block_flag);

    //                        //mode_blockflag.Add(block_flag != 0);

    //                        // only 0 valid for windowtype and transformtype
    //                        var windowtype = new Bit_uint(16);
    //                        var transformtype = new Bit_uint(16);

    //                        osb.Write(windowtype);
    //                        osb.Write(transformtype);

    //                        var mapping = ss1.Read(8);
    //                        osb.Write(mapping);
    //                        if (mapping >= mapping_count) throw new WwiseToOggException("invalid mode mapping");
    //                    }
    //                    osb.Write(new Bit_uint(1, 1));//framing
    //                    // Bit_uint<1> framing(1);
    //                    //  os << framing;

    //                }
    //                //os3.flush_page();

    //                //ms3.Position = 0;
    //                //byte[] msbytes = new byte[ms3.Length];
    //                //ms3.Read(msbytes, 0, msbytes.Length);

    //                packetX = new Packet();
    //                packetX.packet_base = osb.buf();
    //                packetX.packet = 0;
    //                packetX.bytes = osb.bytes();
    //                packetX.b_o_s = 0;
    //                packetX.e_o_s = 0;
    //                packetX.packetno = packnum++;
    //                os.packetin(packetX);
    //            }

    //        }
    //        //os.packetin(packet);

    //        //ss.packetin(packetX);



    //        //var x = si.buffer(31);
    //        //ms.Write(si.data, x, (int)ms.Length);
    //        //si.wrote((int)ms.Length);
    //        //sw.Dispose();


    //        //Page page = new Page();
    //        //if (si.pageout(page) != 1)
    //        //{
    //        //    return false;
    //        //}

    //        //iss.init(page.serialno);
    //        //os.init(page.serialno);



    //        //if (iss.pagein(page) < 0)
    //        //{
    //        //    iss.clear();
    //        //    os.clear();
    //        //    return false;
    //        //}

    //        //Packet packet = new Packet();
    //        //if (iss.packetout(packet) != 1)
    //        //{
    //        //    iss.clear();
    //        //    os.clear();
    //        //    return false;
    //        //}

    //        //Comment vc = new Comment();
    //        //vc.init();
    //        //vc.add("converted from Wwise by dfaudiolib v0.8");

    //        //if (vi.synthesis_headerin(vc, packet) < 0)
    //        //{
    //        //    return false;
    //        //}
    //        //os.packetin(packet);

    //        {
    //            vc.clear();
    //            int total = 0;
    //            Page page = new Page();
    //            while (os.flush(page) != 0)
    //            {
    //                var size = FOutStream.Length;
    //                FOutStream.Write(page.header_base, page.header, page.header_len);
    //                FOutStream.Write(page.body_base, page.body, page.body_len);

    //                if (FOutStream.Length != size + page.header_len + page.body_len)
    //                {
    //                    //  fprintf(stderr,"Cannot write headers to output.\n");
    //                    iss.clear();
    //                    os.clear();
    //                    return false;
    //                }
    //                Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
    //                total += page.header_len + page.body_len;
    //            }
    //            Console.WriteLine("total : " + total);
    //        }
    //        //var x = si.buffer(4096);
    //        //si.wrote(4096);

    //        //os.pagein(p);

    //        //iss.pagein(p);


    //        return false;
    //    }


        
    //}

    internal static class MyBitWriter
    {
        public static void write_32(this Byte[] b, int start, Int32 v)
        {
            write_32_le(b, start, v);
        }

        public static void write_32(this Byte[] b, int start, UInt32 v)
        {
            write_32_le(b, start, v);
        }


        public static void write_32_le(this Byte[] b, int start, UInt32 v)
        {
            for (int i = start; i < start + 4; i++)
            {
                b[i] = (Byte)(v & 0xFF);
                v >>= 8;
            }
        }

        public static void write_64_le(this Byte[] b, int start, Int64 v)
        {
            for (int i = start; i < start + 8; i++)
            {
                b[i] = (Byte)(v & 0xFF);
                v >>= 8;
            }
        }


        public static void write_32_le(this Byte[] b, int start, Int32 v)
        {
            for (int i = start; i < start + 4; i++)
            {
                b[i] = (Byte)(v & 0xFF);
                v >>= 8;
            }
        }


        public static void write_16_le(this Byte[] b, int start, Int16 v)
        {
            for (int i = start; i < start + 2; i++)
            {
                b[i] = (Byte)(v & 0xFF);
                v >>= 8;
            }
        }
        public static void write_16_le(this Byte[] b, int start, UInt16 v)
        {
            for (int i = start; i < start + 2; i++)
            {
                b[i] = (Byte)(v & 0xFF);
                v >>= 8;
            }
        }

        public static UInt32 read_32_be(Byte[] b, int start)
        {
            UInt32 v = 0;
            for (int i = start; i < start + 4; i++)
            {
                v <<= 8;
                v |= b[i];
            }
            return v;
        }


        public static UInt32 read_32_be(Byte[] b)
        {
            UInt32 v = 0;
            for (int i = 0; i < 4; i++)
            {
                v <<= 8;
                v |= b[i];
            }
            return v;
        }

        public static UInt32 read_32_be(Stream AStream)
        {
            byte[] b = new byte[4];
            AStream.Read(b, 0, 4);
            return read_32_be(b);
        }


        public static UInt32 read_32_le(Byte[] b)
        {
            UInt32 v = 0;
            for (int i = 3; i >= 0; i--)
            {
                v <<= 8;
                v |= b[i];
            }
            return v;
        }

        //public static UInt32 read_32_le(Byte[] b, int start)
        //{
        //    UInt32 v = 0;
        //    for (int i = start + 3; i >= start; i--)
        //    {
        //        v <<= 8;
        //        v |= b[i];
        //    }
        //    return v;
        //}

        //public static Int64 read_64_le(Byte[] b, int start)
        //{
        //    Int64 v = 0;
        //    for (int i = start + 7; i >= start; i--)
        //    {
        //        v <<= 8;
        //        v |= b[i];
        //    }
        //    return v;
        //}

        //header

        //public static Int32 read_Signed32_le(Byte[] b, int start)
        //{
        //    Int32 v = 0;
        //    for (int i = start + 3; i >= start; i--)
        //    {
        //        v <<= 8;
        //        v |= b[i];
        //    }
        //    return v;
        //}


        public static UInt32 read_32_le(Stream AStream)
        {
            byte[] b = new byte[4];
            AStream.Read(b, 0, 4);
            return read_32_le(b);
        }

        public static UInt16 read_16_le(Byte[] b)
        {
            UInt16 v = 0;
            for (int i = 1; i >= 0; i--)
            {
                v <<= 8;
                v |= b[i];
            }
            return v;
        }



        public static UInt16 read_16_le(Stream astream)
        {
            byte[] b = new byte[2];
            astream.Read(b, 0, 2);
            return read_16_le(b);
        }



        public static UInt16 read_16_be(Byte[] b)
        {
            UInt16 v = 0;
            for (int i = 0; i < 2; i++)
            {
                v <<= 8;
                v |= b[i];
            }
            return v;
        }

        public static UInt16 read_16_be(Stream astream)
        {
            byte[] b = new byte[2];
            astream.Read(b, 0, 2);
            return read_16_be(b);
        }
    }


}



//        public class oggPage
//        {

////            const int header_bytes = 27;
////            const int max_segments = 255;
////            const int segment_size = 255;

//            protected Int64 granule = 0;
//            bool first = true, continued = false, last = false;

//            Byte[] page_buffer;
//            public int seqno { get; set; }
//            public byte segmentcount { get; set; }
//            public byte[] segmenttable { get; set; }
//            //public byte segmentsize { get; set; }
//            public int serial { get; set; }
//         //   public Int64 granule { get; set; }

//            public oggPage()
//            {
//                //page_buffer = new Byte[header_bytes + max_segments + segment_size * max_segments];
//            }

//            public static List<oggPage> read(Stream astream)
//            {
//                List<oggPage> p = new List<oggPage>();
//                var page = Read(astream);
//                int x = 0;
//                while (page != null)
//                {
//                    x += page.page_buffer.Length;
//                    p.Add(page);
//                    page = Read(astream);
//                }
//                Console.WriteLine(x + (p.Count * 28));
//                return p;
//            }

//            public static void CompressToStream(List<oggPage> list, Stream astream)
//            {
//                List<oggPage> newList = new List<oggPage>();
//                int packNumber = 0;
//                int lastbs = 0;
//                Int64 granpos = 0;
//                foreach (var x in list)
//                {




//                    //for (int i = 0; i < x.segmentcount; i++)
//                    //{
//                    //    int bs = x.segmenttable[i];
//                    //    if (lastbs == 0)
//                    //        lastbs = bs;
//                    //    else
//                    //        granpos += (lastbs + bs) / 4;

//                    //    x.granule = granpos;
//                    //    x.seqno = packNumber++;
//                    //    if (!x.last)
//                    //    {
//                    //        x.Write(astream);
//                    //    }
//                    //}
//               //     x.Write(astream);
//                }


//            }

//            public void Write(Stream astream)
//            {
//                byte[] buffer = new byte[27 + page_buffer.Length + segmentcount];
//                buffer[0] = Convert.ToByte('O');
//                buffer[1] = Convert.ToByte('g');
//                buffer[2] = Convert.ToByte('g');
//                buffer[3] = Convert.ToByte('S');
//                buffer[4] = 0; // stream_structure_version
//                buffer[5] = Convert.ToByte((continued ? 1 : 0) | (first ? 2 : 0) | (last ? 4 : 0));
//                MyBitWriter.write_64_le(buffer, 6, granule);
//                MyBitWriter.write_32(buffer, 14, 1);       // stream serial number
//                MyBitWriter.write_32(buffer, 18, seqno);   // page sequence number
//                MyBitWriter.write_32(buffer, 22, 0);       // checksum (0 for now)
//                buffer[26] = Convert.ToByte(segmentcount);             // segment count
//                for (int i = 0; i < segmenttable.Length; i++)
//                    buffer[27 + i] = segmenttable[i];

//                Array.Copy(page_buffer, 0, buffer, 27 + segmentcount, page_buffer.Length);
//                var checksum = CRC32_2.checksum(buffer);
//                MyBitWriter.write_32(buffer, 22, checksum);
//                astream.Write(buffer, 0, buffer.Length);
//            }

//            public static oggPage Read(Stream astream)
//            {

//                if (astream.Position + 28 > astream.Length)
//                    return null;

//                oggPage result = new oggPage();
//                byte[] header = new byte[27];
//                astream.Read(header, 0, 27);


//                byte x = header[5];
//                result.continued = (x & 1) != 0;
//                result.first = (x & 2) != 0;
//                result.last = (x & 4) != 0;
//                result.granule = MyBitWriter.read_64_le(header, 6);
//                result.serial = MyBitWriter.read_Signed32_le(header, 14);
//                result.seqno = MyBitWriter.read_Signed32_le(header, 18);
//                result.segmentcount = header[26];
//                result.segmenttable = new byte[result.segmentcount];


//                byte[] header2 = new byte[result.segmentcount];
//                astream.Read(header2, 0, result.segmentcount);

//                for (int i = 0; i < result.segmentcount; i++)
//                {
//                    result.segmenttable[i] = header2[i];
//                }

//                List<byte> allb = new List<byte>();
//                foreach (var b in result.segmenttable)
//                {
//                    byte[] ab = new byte[b];
//                    astream.Read(ab, 0, b);
//                    allb.AddRange(ab);
//                }
//                result.page_buffer = allb.ToArray();

//                //result.segmentsize = header[27];


//              //  int buffSize = result.segmentcount * result.segmentsize;
//              //  result.page_buffer = new byte[buffSize];
//               // astream.Read(result.page_buffer, 0, buffSize);
//                return result;

//            }

//            /*  public byte[] header;//byte[]
//        public int header_len;
//        public byte[] body;//byte[]
//        public int body_len;*/


//            /*
//                    MyBitWriter.write_64_le(page_buffer, 6, granule);
//                    MyBitWriter.write_32(page_buffer, 14, 1);       // stream serial number
//                    MyBitWriter.write_32(page_buffer, 18, seqno);   // page sequence number
//                    MyBitWriter.write_32(page_buffer, 22, 0);       // checksum (0 for now)
//                    page_buffer[26] = Convert.ToByte(segments);             // segment count
//                    // lacing values
//                    for (int i = 0, bytes_left = payload_bytes; i < segments; i++)
//                    {
//                        if (bytes_left >= segment_size)
//                        {
//                            bytes_left -= segment_size;
//                            page_buffer[27 + i] = segment_size;
//                        }
//                        else
//                        {
//                            page_buffer[27 + i] = Convert.ToByte(bytes_left);
//                        }
//                    }
//                    var checksum = CRC32_2.checksum(page_buffer, header_bytes + segments + payload_bytes);

//                    MyBitWriter.write_32(page_buffer, 22, checksum);

//                    //instream.Write(page_buffer, 0, page_buffer.Length);
//                    // output to ostream
//                    for (int i = 0; i < header_bytes + segments + payload_bytes; i++)
//                    {
//                        instream.WriteByte(page_buffer[i]);
//                    }*/


//        }
