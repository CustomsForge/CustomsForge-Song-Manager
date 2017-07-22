DLogNet Usage Notes:

Place the following into the mainForm after InitializeComponent();

     this.FormClosed += delegate
         {
             DLogger.Instance.RemoveTargetNotifyIcon(DLogger.Notifier);
             notifierMain.Visible = false;
             notifierMain.Dispose();
             notifierMain.Icon = null;
             Dispose();
         };



       private void InitLogger()
        {
            tbLog.Clear();
            tbLog.Text = String.Empty;
            DLogger.Instance = null; // this is key to proper function
            DLogger.Instance.AddTargetFile(Constants.AppLogPath);
            DLogger.Instance.AddTargetTextBox(tbLog);
            DLogger.Log("Initialized application logger ...");
        }
		
		
