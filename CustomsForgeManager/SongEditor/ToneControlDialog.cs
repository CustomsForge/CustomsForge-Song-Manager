using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager.SongEditor
{
    public partial class ToneControlDialog : Form
    {
        public ToneControlDialog()
        {
            InitializeComponent();
            toneControl1.Init();
        }

        public Tone2014 Tone
        {
            get
            {
                return toneControl1.Tone;
            }
            set
            {
                toneControl1.Tone = value;
            }
        }
    }
}
