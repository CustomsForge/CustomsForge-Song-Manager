using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// Usage in frmMain comes after InitializeComponent();
// DLogger.Instance.AddTargetFile(Constants.AppLogPath);
// DLogger.Instance.AddTargetTextBox(tbLog);            
// DLogger.Log("Initialed application logger ...");


namespace DLogNet
{
    public class DLogger
    {
        private List<DLogMessage> logEntries;
        private List<TextBox> targetTextBoxes = new List<TextBox>();
        private List<FileInfo> targetFiles = new List<FileInfo>();
        private List<NotifyIcon> targetNotifyIcons = new List<NotifyIcon>();
        private List<ProgressBar> targetProgressBars = new List<ProgressBar>();
        private List<ToolStripProgressBar> targetToolStripProgressBars = new List<ToolStripProgressBar>();

        private static DLogger _instance;
        public static DLogger Instance
        {
            get { return _instance ?? (_instance = new DLogger()); }
            set { _instance = value; }
        }

        public static void Log(string message)
        {
            Instance.Write(message);
        }

        private static NotifyIcon _notifyIcon;
        public static NotifyIcon Notifier
        {
            get { return _notifyIcon ?? (_notifyIcon = new NotifyIcon()); }
            set { _notifyIcon = value; }
        }

        public List<NotifyIcon> TargetNotifyIcons
        {
            get { return targetNotifyIcons; }
        }

        public List<FileInfo> TargetFiles
        {
            get { return targetFiles; }
        }

        public List<TextBox> TargetTextBoxes
        {
            get { return targetTextBoxes; }
        }

        public List<ProgressBar> TargetProgressBars
        {
            get { return targetProgressBars; }
        }

        public List<ToolStripProgressBar> TargetToolStripProgressBars
        {
            get { return targetToolStripProgressBars; }
        }

        /// <summary>
        /// Instanciates Log class
        /// </summary>
        public DLogger()
        {
            logEntries = new List<DLogMessage>();
        }


        /// <summary>
        /// Adds TextBox control for log output target control list
        /// </summary>
        /// <param name="textBox">TextBox to add</param>
        public void AddTargetTextBox(TextBox textBox)
        {
            if (targetTextBoxes == null)
                targetTextBoxes = new List<TextBox>();
            targetTextBoxes.Add(textBox);
        }

        /// <summary>
        /// Removes TextBox control from log output
        /// </summary>
        /// <param name="textBox">TextBox to remove</param>
        public void RemoveTargetTextBox(TextBox textBox)
        {
            if (targetTextBoxes.Contains(textBox))
                targetTextBoxes.Remove(textBox);
        }

        /// <summary>
        /// Adds file by path for log output target file list
        /// </summary>
        /// <param name="path">Path of the file</param>
        public void AddTargetFile(string path)
        {
            if (targetFiles == null)
                targetFiles = new List<FileInfo>();
            FileInfo newTargetFile = new FileInfo(path);

            if (newTargetFile.Directory != null && !newTargetFile.Directory.Exists)
                Directory.CreateDirectory(newTargetFile.Directory.FullName);

            // commented out ... not a good idea to do this in middle of a process
            // automatically delete and recreate log file when it gets too big
            //if (newTargetFile.Exists && newTargetFile.Length / 1024 > 1024)
            //    File.Delete(path);

            if (!newTargetFile.Exists)
            {
                StreamWriter sw = File.CreateText(newTargetFile.FullName);
                sw.Flush();
                sw.Close();
            }

            bool exists = false;
            foreach (FileInfo targetFile in targetFiles)
            {
                if (targetFile.FullName == path)
                    exists = true;
            }

            if (!exists)
                targetFiles.Add(newTargetFile);
        }

        /// <summary>
        /// Removes file from log output
        /// </summary>
        /// <param name="path">Log file path</param>
        public void RemoveTargetFile(string path)
        {
            foreach (FileInfo targetFile in targetFiles.ToList())
            {
                if (targetFile.FullName == path)
                    targetFiles.Remove(targetFile);
            }
        }

        /// <summary>
        /// Adds NotifyIcon control for log output 
        /// </summary>
        /// <param name="notifyIcon">NotifyIcon to add</param>
        public void AddTargetNotifyIcon(NotifyIcon notifyIcon)
        {
            if (targetNotifyIcons == null)
                targetNotifyIcons = new List<NotifyIcon>();
            targetNotifyIcons.Add(notifyIcon);
        }

        /// <summary>
        /// Removes Notify Icon control from log output
        /// </summary>
        /// <param name="notifyIcon">Notify Icon control to remove</param>
        public void RemoveTargetNotifyIcon(NotifyIcon notifyIcon)
        {
            if (targetNotifyIcons.Contains(notifyIcon))
                targetNotifyIcons.Remove(notifyIcon);
        }

        /// <summary>
        /// Adds file by FileInfo type variable for log output target file list
        /// </summary>
        /// <param name="file">FileInfo type variable</param>
        public void AddTargetFile(FileInfo file)
        {
            AddTargetFile(file.FullName);
        }

        /// <summary>
        /// Adds Progress Bar control for log output
        /// </summary>
        /// <param name="progressBar">Progress Bar control to add</param>
        public void AddProgressBar(ProgressBar progressBar)
        {
            if (targetProgressBars == null)
                targetProgressBars = new List<ProgressBar>();
            targetProgressBars.Add(progressBar);
        }

        /// <summary>
        /// Removes Progress Bar control from log output
        /// </summary>
        /// <param name="progressBar">Progress Bar control to remove</param>
        public void RemoveTargetProgressBar(ProgressBar progressBar)
        {
            if (targetProgressBars.Contains(progressBar))
                targetProgressBars.Remove(progressBar);
        }

        /// <summary>
        /// Adds ToolStripProgressBar control for log output
        /// </summary>
        /// <param name="progressBar">ToolStripProgressBar control to add</param>
        public void AddToolStripProgressBar(ToolStripProgressBar progressBar)
        {
            if (targetToolStripProgressBars == null)
                targetToolStripProgressBars = new List<ToolStripProgressBar>();
            targetToolStripProgressBars.Add(progressBar);
        }

        /// <summary>
        /// Removes ToolStripProgressBar control from log output
        /// </summary>
        /// <param name="progressBar">ToolStripProgressBar control to remove</param>
        public void RemoveTargetProgressBar(ToolStripProgressBar progressBar)
        {
            if (targetToolStripProgressBars.Contains(progressBar))
                targetToolStripProgressBars.Remove(progressBar);
        }

        /// <summary>
        /// Outputs log to each target objects
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="progress">(Optional) Progress value (for ProgressBar type controls)</param>
        public void Write(string message, int progress = -1)
        {
            DLogMessage msg = new DLogMessage(message);
            if (logEntries == null)
                logEntries = new List<DLogMessage>();
            logEntries.Add(msg);

            try
            {
                foreach (var entry in logEntries)
                {
                    if (targetTextBoxes != null)
                    {
                        foreach (Control control in targetTextBoxes)
                        {
                            Control myControl = control;
                            DLogMessage myLogEntry = entry;
                            control.InvokeIfRequired(delegate
                                {
                                    myControl.Text += myLogEntry.GetFormatted() + Environment.NewLine;
                                    if (myControl is TextBox)
                                    {
                                        ((TextBox)myControl).SelectionStart = ((TextBox)myControl).TextLength;
                                        ((TextBox)myControl).ScrollToCaret();
                                    }
                                });
                        }
                    }
                    if (targetFiles != null)
                    {
                        foreach (FileInfo targetFile in targetFiles)
                        {
                            // workaround FileInfo sometimes returns false when file exists
                            var targetPath = Path.Combine(targetFile.DirectoryName, targetFile.Name);
                            if (!File.Exists(targetPath)) // targetFile.Exists))
                            {
                                using (StreamWriter sw = targetFile.CreateText())
                                {
                                    sw.WriteLine("Log started");
                                    sw.Flush();
                                    sw.Close();
                                }
                            }

                            using (StreamWriter sw = targetFile.AppendText())
                            {
                                sw.WriteLine(entry.GetFormatted());
                                sw.Flush();
                                sw.Close();
                            }
                        }
                    }
                    if (targetNotifyIcons != null)
                    {
                        foreach (NotifyIcon notifyIcon in targetNotifyIcons)
                        {
                            ToolTipIcon icon;
                            notifyIcon.BalloonTipText = entry.GetFormatted();
                            notifyIcon.Visible = true;
                            if (entry.Message == null) continue;
                            if (entry.Message.ToLower().Contains("error"))
                                icon = ToolTipIcon.Error;
                            else
                                icon = ToolTipIcon.Info;

                            notifyIcon.ShowBalloonTip(1, "Information", entry.Message, icon);
                        }
                    }

                    if (progress > -1)
                    {
                        if (targetProgressBars != null)
                        {
                            foreach (ProgressBar progressBar in targetProgressBars)
                            {
                                ProgressBar bar = progressBar;
                                bar.InvokeIfRequired(delegate
                                    { bar.Value = progress; });
                            }
                        }

                        if (targetToolStripProgressBars != null)
                        {
                            foreach (ToolStripProgressBar toolStripProgressBar in targetToolStripProgressBars)
                            {
                                toolStripProgressBar.Value = progress;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // some intermitent error shows up here
                Debug.Write("DLogger: " + ex.Message);
            }

            logEntries = new List<DLogMessage>();
        }
    }
}
