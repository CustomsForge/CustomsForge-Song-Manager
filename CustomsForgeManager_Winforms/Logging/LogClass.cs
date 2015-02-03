using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsForgeManager_Winforms.Logging
{
    public class Log
    {
        private DateTime timestamp;
        private string message;


        private Log()
        {
            timestamp = DateTime.Now;
            message = string.Empty;
        }

        public Log(string logMessage) : this()
        {
            message = logMessage;
        }

        private string GetFormatted()
        {
            return string.Format("[{0}]: {1}",timestamp.ToShortDateString(), message);
        }

        public override string ToString()
        {
            return GetFormatted();
        }
    }
}
