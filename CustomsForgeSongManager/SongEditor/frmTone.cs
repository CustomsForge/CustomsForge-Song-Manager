using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class frmTone : Form
    {
        public frmTone()
        {
            InitializeComponent();
            toneControl1.Init();
        }

        public Tone2014 Tone
        {
            get { return toneControl1.Tone; }
            set { toneControl1.Tone = value; }
        }
    }
}