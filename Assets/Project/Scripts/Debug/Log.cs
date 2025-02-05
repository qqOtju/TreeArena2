using System;
using System.Collections.Generic;

namespace Project.Scripts.Debug
{
    public class Log
    {
        public List<LogMessage> Messages { get; } = new ();

        public void AddMessage(LogType logType, string message)
        {
            Messages.Add(new LogMessage
            {
                LogType = logType,
                Message = message,
                Time = DateTime.Now,
                Read = false
            });
        }
    }
}