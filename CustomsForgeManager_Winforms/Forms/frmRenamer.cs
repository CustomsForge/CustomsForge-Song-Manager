using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmRenamer : Form
    {
        private Logging.Log myLog;

        private frmRenamer()
        {
            
        }

        public frmRenamer(Logging.Log myLog) 
        {
            // TODO: Complete member initialization
            this.myLog = myLog;
            InitializeComponent();
            myLog.Write("Renamer opened");
        }
    }
}
