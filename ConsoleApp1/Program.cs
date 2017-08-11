using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace ConsoleApp1
{
    class Program: ICalculator 
    {
        public string Mode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public static ICalculator  _calculator;
        public event EventHandler PowerUp;

        static void Main(string[] args)
        {
            _calculator = Substitute.For<ICalculator >();
            
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }
    }
    public interface ICalculator 
    {
        int Add(int a, int b);
        int Multiply(int a, int b);
        string Mode { get; set; }
        event EventHandler PowerUp;
    }
}
