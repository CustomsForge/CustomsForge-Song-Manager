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

        public string GetFormatted()
        {
            return string.Format("[{0:d/M/yyyy HH:mm:ss}]: {1}", TimeStamp, Message);
        }
    }
}
