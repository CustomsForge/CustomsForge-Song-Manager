using CFMAudioTools.Ogg;
using CFMAudioTools.Vorbis;
using System;
using System.IO;

namespace CFMAudioTools
{
    public struct OggRevorb
    {
        Stream FInStream, FOutStream;

        public OggRevorb(Stream inStream, Stream outStream)
        {
            FInStream = inStream;
            FOutStream = outStream;
        }

        public bool Execute()
        {
            SyncState sync_in = new SyncState();
            sync_in.init();
            StreamState stream_in = new StreamState();
            StreamState stream_out = new StreamState();
            bool g_failed = false;
            Info vi = new Info();
            vi.init();

            Packet packet = new Packet();
            Page page = new Page();

            if (copy_headers(sync_in, stream_in, stream_out, vi))
            {
                Int64 granpos = 0, packetnum = 0;
                int lastbs = 0;

                while (true)
                {
                    int eos = 0;
                    while (eos == 0)
                    {
                        int res = sync_in.pageout(page);
                        if (res == 0)
                        {
                            var x = sync_in.buffer(4096);

                            int numread = FInStream.Read(sync_in.data, x, 4096); //fread(buffer, 1, 4096, fi);
                            if (numread > 0)
                                sync_in.wrote(numread);
                            else
                                eos = 2;
                            continue;
                        }

                        if (res < 0)
                        {
                            Console.WriteLine("Warning: Corrupted or missing data in bitstream.");
                            g_failed = true;
                        }
                        else
                        {
                            if (page.eos() > 0)
                                eos = 1;
                            stream_in.pagein(page);

                            while (true)
                            {
                                res = stream_in.packetout(packet);
                                if (res == 0)
                                    break;
                                if (res < 0)
                                {
                                    Console.WriteLine("Warning: Bitstream error.");
                                    g_failed = true;
                                    continue;
                                }

                                /*
                                if (packet.granulepos >= 0) {
                                  granpos = packet.granulepos + logstream_startgran;
                                  packet.granulepos = granpos;
                                }
                                */
                                int bs = vi.blocksize(packet);
                                if (lastbs != 0)
                                    granpos += (lastbs + bs) / 4;
                                lastbs = bs;

                                packet.granulepos = granpos;
                                packet.packetno = packetnum++;
                                if (packet.e_o_s == 0)
                                {
                                    stream_out.packetin(packet);

                                    Page opage = new Page();
                                    while (stream_out.pageout(opage) != 0)
                                    {
                                        var size = FOutStream.Length;
                                        FOutStream.Write(opage.header_base, opage.header, opage.header_len);
                                        FOutStream.Write(opage.body_base, opage.body, opage.body_len);

                                        if (FOutStream.Length != size + opage.header_len + opage.body_len)
                                        {
                                            Console.WriteLine("Unable to write page to output.");
                                            eos = 2;
                                            g_failed = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (eos == 2)
                        break;

                    {
                        packet.e_o_s = 1;
                        stream_out.packetin(packet);
                        Page opage = new Page();
                        while (stream_out.flush(opage) != 0)
                        {
                            var size = FOutStream.Length;
                            FOutStream.Write(opage.header_base, 0, opage.header_len);
                            FOutStream.Write(opage.body_base, 0, opage.body_len);

                            if (FOutStream.Length != size + opage.header_len + opage.body_len)
                            {
                                Console.WriteLine("Unable to write page to output.\n");
                                g_failed = true;
                                break;
                            }
                        }
                        stream_in.clear();
                        break;
                    }
                }

                stream_out.clear();

            }
            vi.clear();
            sync_in.clear();
            return !g_failed;
        }


        private bool copy_headers(SyncState si, StreamState iss, StreamState os, Info vi)
        {
            var x = si.buffer(4096);
            int numread = FInStream.Read(si.data, x, 4096);
            si.wrote(numread);

            Page page = new Page();
            if (si.pageout(page) != 1)
            {
                return false;
            }

            iss.init(page.serialno);
            os.init(page.serialno);

            if (iss.pagein(page) < 0)
            {
                iss.clear();
                os.clear();
                return false;
            }

            Packet packet = new Packet();
            if (iss.packetout(packet) != 1)
            {
                iss.clear();
                os.clear();
                return false;
            }

            Comment vc = new Comment();
            vc.init();

            if (vi.synthesis_headerin(vc, packet) < 0)
            {
                vc.clear();
                iss.clear();
                os.clear();
                return false;
            }

            os.packetin(packet);


            int i = 0;
            while (i < 2)
            {
                int res = si.pageout(page);

                if (res == 0)
                {
                    x = si.buffer(4096);
                    numread = FInStream.Read(si.data, x, 4096);

                    if (numread == 0 && i < 2)
                    {
                        Console.WriteLine("Headers are damaged, file is probably truncated.");

                        vc.clear();
                        iss.clear();
                        os.clear();
                        return false;
                    }
                    si.wrote(4096);
                    continue;
                }

                if (res == 1)
                {
                    iss.pagein(page);
                    while (i < 2)
                    {
                        res = iss.packetout(packet);
                        if (res == 0)
                            break;
                        if (res < 0)
                        {
                            Console.WriteLine("Secondary header is corrupted.");
                            vc.clear();
                            iss.clear();
                            os.clear();
                            return false;
                        }
                        vi.synthesis_headerin(vc, packet);
                        os.packetin(packet);
                        i++;
                    }
                }
            }

            vc.clear();
          //  int total = 0;
            while (os.flush(page) != 0)
            {
                var size = FOutStream.Length;
                FOutStream.Write(page.header_base, page.header, page.header_len);
                FOutStream.Write(page.body_base, page.body, page.body_len);

                if (FOutStream.Length != size + page.header_len + page.body_len)
                {
                    Console.WriteLine("Cannot write headers to output.");
                    iss.clear();
                    os.clear();
                    return false;
                }
            //    Console.WriteLine(String.Format("head : {0}, body : {1}.", page.header_len, page.body_len));
            //    total += page.header_len + page.body_len;
            }
            //Console.WriteLine("total : " + total);
            return true;
        }
    }


}
