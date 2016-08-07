using System;
using System.Collections.Generic;

namespace Estimatorx.Core
{
    public class LogEvent
    {
        public LogEvent()
        {
            Properties = new Dictionary<string, string>();
        }

        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string Correlation { get; set; }

        public LogError Exception { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
