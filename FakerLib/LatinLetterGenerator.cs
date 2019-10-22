using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    class ConvenienceRandom
    {
        static Random shared = new Random();
        public static int Next(int minValue, int maxValue)
        {
            return shared.Next(minValue, maxValue);
        }
    }
    abstract class CharGenerator : IGenerator
    {
        public abstract object CreateInstance();
        public abstract Type GeneratedType();
    }
    class LatinLetterGenerator : CharGenerator
    {
        public override object CreateInstance()
        {
            return Convert.ToChar(ConvenienceRandom.Next(65, 122));
        }

        public override Type GeneratedType()
        {
            return typeof(char);
        }
    }
}
