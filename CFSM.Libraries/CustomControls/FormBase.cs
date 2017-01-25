using System;
using System.Windows.Forms;

namespace CustomControls
{
    public class FormBase : Form
    {
        protected FormBase()
        {
            ShowInTaskbar = false;
        }

        public void Invoke(Action a)
        {
            ControlHelpers.Invoke(this, a);
        }

    }
}
