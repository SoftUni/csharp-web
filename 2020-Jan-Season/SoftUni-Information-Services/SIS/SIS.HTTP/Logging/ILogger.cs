using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Logging
{
    public interface ILogger
    {
        void Log(string message);
    }
}
