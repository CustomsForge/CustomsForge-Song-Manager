namespace GenTools
{
    public static class UiExtensions
    {
        // usage: move this code to a Form or UC to get all Controls
        /*
         
        private void ToggleControls(Control container, bool enable)
        {
            foreach (Control c in container.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    if (c is StatusStrip)
                        Debug.WriteLine("Ignoring StatusStrip Controls");
                    else if (c == scMain.Panel2)
                        Debug.WriteLine("Ignoring Panel2 Controls");
                    else
                        ToggleControls(c, enable);
                }
                else if (c is Button || c is TextBox || c is ListView || c is ComboBox || c is LinkLabel)
                    c.Enabled = enable;
            }
        }

        private void ToggleUI(bool enable)
        {
            ToggleControls(this, enable);
        }
       
          */
    }
}
