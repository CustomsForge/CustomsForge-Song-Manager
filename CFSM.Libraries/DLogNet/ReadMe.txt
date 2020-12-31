DLogNet Usage Notes:
======================

Define a public global variable in Constants class

  public DLogNet AppLogger;

======================
Place the following into the mainForm after InitializeComponent();

  // event handler for AppLogger
  this.Closing += (object sender, CancelEventArgs e) =>
  {
      Constants.AppLogger.RemoveTargetNotifyIcon(Constants.AppLogger.Notifier);
      notifierMain.Visible = false;
      notifierMain.Icon.Dispose();
      notifierMain.Dispose();
      Application.DoEvents();
  };
           
  InitLogger();
         
======================
Add the following method

  private void InitLogger()
  {
      txtLog.Clear();
      AppLogger = new AppLogger();
      Constants.AppLogger.AddTargetFile(logPath);
      Constants.AppLogger.AddTargetTextBox(txtLog);
      Constants.AppLogger.AddTargetNotifyIcon(notifierMain);
      logger.Log("Initialized logger ...");
  }
		
		
