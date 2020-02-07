using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.HTTP.Logging
{
    public class FileLogger : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllLines("log.txt", new[] { message });
        }
    }
}
