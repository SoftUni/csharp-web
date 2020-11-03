using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Service
{
    public class InstanceCounter : IInstanceCounter
    {
        private static int instances;

        public InstanceCounter()
        {
            instances++;
        }

        public int Instances => instances;
    }
}
