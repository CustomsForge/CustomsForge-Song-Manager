namespace CFMAudioTools.Ogg
{
    public class Page
    {
        public byte[] header_base;
        public int header;
        public int header_len;
        public byte[] body_base;
        public int body;
        public int body_len;

        internal int version()
        {
            return header_base[header + 4] & 0xff;
        }
        internal int continued()
        {
            return (header_base[header + 5] & 0x01);
        }
        public int bos()
        {
            return (header_base[header + 5] & 0x02);
        }
        public int eos()
        {
            return (header_base[header + 5] & 0x04);
        }
        public long granulepos()
        {
            long foo = header_base[header + 13] & 0xff;
            foo = (foo << 8) | (uint)(header_base[header + 12] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 11] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 10] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 9] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 8] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 7] & 0xff);
            foo = (foo << 8) | (uint)(header_base[header + 6] & 0xff);
            return (foo);
        }
        public int serialno
        {
            get
            {
                return (header_base[header + 14] & 0xff) |
                    ((header_base[header + 15] & 0xff) << 8) |
                    ((header_base[header + 16] & 0xff) << 16) |
                    ((header_base[header + 17] & 0xff) << 24);
            }
        }
        internal int pageno()
        {
            return (header_base[header + 18] & 0xff) |
                ((header_base[header + 19] & 0xff) << 8) |
                ((header_base[header + 20] & 0xff) << 16) |
                ((header_base[header + 21] & 0xff) << 24);
        }

        internal void checksum()
        {
            uint crc_reg = 0;
            uint a, b;

            for (int i = 0; i < header_len; i++)
            {
                a = header_base[header + i] & 0xffu;
                b = (crc_reg >> 24) & 0xff;
                crc_reg = (crc_reg << 8) ^ CRC32.crc_lookup[a ^ b];
            }
            for (int i = 0; i < body_len; i++)
            {
                a = body_base[body + i] & 0xffu;
                b = (crc_reg >> 24) & 0xff;
                crc_reg = (crc_reg << 8) ^ CRC32.crc_lookup[a ^ b];
            }
            header_base[header + 22] = (byte)crc_reg/*&0xff*/;
            header_base[header + 23] = (byte)(crc_reg >> 8)/*&0xff*/;
            header_base[header + 24] = (byte)(crc_reg >> 16)/*&0xff*/;
            header_base[header + 25] = (byte)(crc_reg >> 24)/*&0xff*/;
        }
    }
}
