using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Service
{
    public interface IInstanceCounter
    {
        int Instances { get; }
    }
}
