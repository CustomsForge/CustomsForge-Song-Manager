using System;

namespace DLogNet
{
    class DLogMessage
    {
        private readonly DateTime _timestamp;
        private readonly string _message;

        private DateTime TimeStamp { get { return _timestamp; } }
        public string Message { get { return _message; } }

        public DLogMessage(string message)
            : this()
        {
            _message = message;
        }

        private DLogMessage()
        {
            _timestamp = DateTime.Now;
            _message = "";
        }

        // sortable string yyyy/MM/dd
        // quasi ISO8601 DateTime format
        public string GetFormatted()
        {
            if (String.IsNullOrEmpty(Message))
                return String.Empty;

            return string.Format("[{0:yyyy/MM/dd HH:mm:ss}]: {1}", TimeStamp, Message);
        }
    }
}
