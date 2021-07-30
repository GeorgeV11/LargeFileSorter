using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortStrings
{
    public class TickCounter : IDisposable
    {
        protected int _Result;
        protected int _Start;
        public int Result
        {
            get{
                return _Result;
            }
        }
        public TickCounter()
        {
            Start();
        }
        TickCounter Start()
        {
            _Start = Environment.TickCount;
            return this;
        }
        public void Dispose()
        {
            var tc = Environment.TickCount;
            _Result = tc - _Start;
        }

    }
}
