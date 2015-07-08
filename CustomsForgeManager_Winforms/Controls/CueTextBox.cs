using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Controls
{
    class CueTextBox : TextBox
    {
        private string cueText = "";
        public string CueText
        {
            get { return cueText; }
            set { cueText = value; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ForeColor = Color.Gray;
            Text = cueText;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            ForeColor = Color.Black;
            if(Text == CueText) Text = string.Empty;
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (Text != string.Empty) return;

            ForeColor = Color.Gray;
            Text = cueText;
        }
    }
}
