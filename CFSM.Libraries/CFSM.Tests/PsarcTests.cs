#define PSARCTEST

using System;
using CFSM.RSTKLib.PSARC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PSARC = RocksmithToolkitLib.PSARC.PSARC;

namespace CFSM.Tests
{
#if PSARCTEST
    [TestClass]
    public class PSARCTest
    {
        [TestMethod]
        //TestTOC_RSTK will fail
        public void TestTOC_RSTKLib()
        {
            Random r = new Random();

            using (var l = new NoCloseStreamList())
            {
                NoCloseStream pwrite = l.NewStream();
                using (var p = new PSARC())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        byte[] b = new byte[50];
                        r.NextBytes(b);

                        MemoryStream ms = l.NewStream();
                        ms.Write(b, 0, 50);
                        p.AddEntry("test" + i, ms);
                    }
                    p.Write(pwrite);
                }
                pwrite.Position = 0;
                using (var p = new PSARC())
                {
                    p.Read(pwrite);

                    byte[] b = new byte[50];
                    r.NextBytes(b);

                    MemoryStream ms = l.NewStream();
                    ms.Write(b, 0, 50);
                    p.AddEntry("test10", ms);

                    pwrite = l.NewStream();
                    p.Write(pwrite);
                }
                pwrite.Position = 0;

                using (var p = new PSARC())
                {
                    p.Read(pwrite, true);
                    for (int i = 0; i < 11; i++)
                    {
                        var entry = p.TOC.Find(e => e.Name == "test" + i);
                        Assert.AreNotEqual(entry, null, string.Format("'Test {0}' not found", i));
                    }
                }
            }
        }

        [TestMethod]
        public void TestTOC_CFSM_RSTKLib()
        {
            Random r = new Random();
            using (var l = new NoCloseStreamList())
            {
                NoCloseStream pwrite = l.NewStream();

                using (var p = new PSARC())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        byte[] b = new byte[50];
                        r.NextBytes(b);

                        MemoryStream ms = l.NewStream();
                        ms.Write(b, 0, 50);
                        p.AddEntry("test" + i, ms);
                    }
                    p.Write(pwrite);
                }
                pwrite.Position = 0;

                using (var p = new PSARC())
                {
                    p.Read(pwrite);

                    byte[] b = new byte[50];
                    r.NextBytes(b);

                    MemoryStream ms = l.NewStream();
                    ms.Write(b, 0, 50);
                    p.AddEntry("test10", ms);

                    pwrite = l.NewStream();
                    p.Write(pwrite);
                }

                pwrite.Position = 0;
                using (var p = new PSARC())
                {
                    p.Read(pwrite, true);
                    for (int i = 0; i < 11; i++)
                    {
                        var entry = p.TOC.Find(e => e.Name == "test" + i);
                        Assert.AreNotEqual(entry, null, string.Format("'Test {0}' not found", i));
                    }
                }
            }
        }
    }
#endif

}