using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Utilities;

namespace CustomsForgeManager_Winforms.Logging
{
    public class LogMessage
    {
        private readonly DateTime _timestamp;
        private readonly string _message;

        public DateTime TimeStamp { get { return _timestamp; } }
        public string Message { get { return _message; } }

        public LogMessage(string message) : this()
        {
            _message = message;
        }

        private LogMessage()
        {
            _timestamp = DateTime.Now;
            _message = "";
        }

        public string GetFormatted()
        {
            return string.Format("[{0:d/M/yyyy HH:mm:ss}]: {1}", TimeStamp, Message);
        }
    }

    public class Log
    {
        private List<LogMessage> logEntries; 

        private List<TextBox> targetTextBoxes;
        private List<FileInfo> targetFiles;
        private List<NotifyIcon> targetNotifyIcons;

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

        /// <summary>
        /// Instanciates Log class
        /// </summary>
        public Log()
        {
            logEntries = new List<LogMessage>();
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
        /// Adds file by path for log output target file list
        /// </summary>
        /// <param name="path">Path of the file</param>
        public void AddTargetFile(string path)
        {
            if (targetFiles == null)
                targetFiles = new List<FileInfo>();
            FileInfo newTargetFile = new FileInfo(path);
            targetFiles.Add(newTargetFile);
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
        /// Adds file by FileInfo type variable for log output target file list
        /// </summary>
        /// <param name="file">FileInfo type variable</param>
        public void AddTargetFile(FileInfo file)
        {
            AddTargetFile(file.FullName);
        }


        /// <summary>
        /// Outputs log to controls and files (if previously set)
        /// </summary>
        /// <param name="message">Log message</param>
        public void Write(string message, bool popup = true)
        {
            LogMessage msg = new LogMessage(message);
            if (logEntries == null)
                logEntries = new List<LogMessage>();
            logEntries.Add(msg);

            foreach (var entry in logEntries)
            {
                if (targetTextBoxes != null)
                {
                    foreach (Control control in targetTextBoxes)
                    {
                        Control myControl = control;
                        LogMessage myLogEntry = entry;
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
                        if (!targetFile.Exists)
                        {
                            using (StreamWriter sw = targetFile.CreateText())
                            {
                                sw.WriteLine("Log started");
                            }
                        }

                        using (StreamWriter sw = targetFile.AppendText())
                        {
                            sw.WriteLine(entry.GetFormatted());
                        }
                    }
                }
                if(popup)
                {
                    if (targetNotifyIcons != null)
                    {
                        foreach (NotifyIcon notifyIcon in targetNotifyIcons)
                        {
                            ToolTipIcon icon;
                            notifyIcon.BalloonTipText = entry.GetFormatted();
                            notifyIcon.Visible = true;
                            if (entry.Message.ToLower().Contains("error"))
                                icon = ToolTipIcon.Error;
                            else
                                icon = ToolTipIcon.Info;

                            notifyIcon.ShowBalloonTip(1, "Information", entry.Message, icon);
                        }
                    }
                }
            }
            logEntries = new List<LogMessage>();
        }

        public void RemoveTargetNotifyIcon(NotifyIcon notifyIconMain)
        {
            if (targetNotifyIcons != null)
            {
                targetNotifyIcons.Remove(notifyIconMain);
            }
        }
    }
}
