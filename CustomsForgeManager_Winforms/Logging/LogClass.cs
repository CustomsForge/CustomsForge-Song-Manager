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

        private List<Control> targetControls;
        private List<FileInfo> targetFiles;

        public List<Control> TargetControls
        {
            get { return targetControls; }
        }

        public List<FileInfo> TargetFiles
        {
            get { return targetFiles; }
        }

        /// <summary>
        /// Instanciates Log class
        /// </summary>
        public Log()
        {
            logEntries = new List<LogMessage>();
        }

        
        /// <summary>
        /// Adds control for log output target control list
        /// </summary>
        /// <param name="control">Control to add</param>
        public void AddTargetControls(Control control)
        {
            if (targetControls == null)
                targetControls = new List<Control>();
            targetControls.Add(control);
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
        public void Write(string message)
        {
            LogMessage msg = new LogMessage(message);
            if (logEntries == null)
                logEntries = new List<LogMessage>();
            logEntries.Add(msg);

            foreach (var entry in logEntries)
            {
                if (targetControls != null)
                {
                    foreach (Control control in targetControls)
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
            }
            logEntries = new List<LogMessage>();
        }

        
    }
}
