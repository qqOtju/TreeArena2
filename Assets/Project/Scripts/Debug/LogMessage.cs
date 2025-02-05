using System;

namespace Project.Scripts.Debug
{
    public struct LogMessage
    {
        public LogType LogType;
        public string Message;
        public DateTime Time;
        public bool Read;
    }
}