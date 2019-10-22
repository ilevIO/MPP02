using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    class NumericGenerator : IGenerator
    {
        private static Random random = new Random();
        public object CreateInstance()
        {
            return random.Next();
        }

        public Type GeneratedType()
        {
            return typeof(int);
        }
    }
    class IntGenerator : IGenerator
    {

        /*static*/ public object CreateInstance()
        {
            return (Int32)(ConvenienceRandom.Next(0, Int32.MaxValue));
        }

        /*static*/ public Type GeneratedType()
        {
            return typeof(Int32);
        }
    }
}
