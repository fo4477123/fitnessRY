using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CForTest : ICalculator 
    {
        public string Mode { get ; set; }

        public event EventHandler PowerUp;

        public int Add(int a, int b)
        {
            return a + b;
        }


        public int greater(int from, int adder, int cnt)
        {
            object locker = 0;
            Parallel.For(0, cnt, (i,loopState) => {
                lock (locker)
                {
                    from += adder;
                }
            });
            return from;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }
    }
}
