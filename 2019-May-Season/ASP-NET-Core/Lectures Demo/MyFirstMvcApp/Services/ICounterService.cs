using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Services
{
    public interface ICounterService
    {
        int GetCount();
    }

    public class CounterService : ICounterService
    {
        private static int instances = 0;

        public CounterService()
        {
            instances++;
        }

        public int GetCount()
        {
            return instances;
        }
    }
}
