using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ConvenienceRandom
    {
        static Random shared = new Random();
        public static int Next(int minValue, int maxValue)
        {
            return shared.Next(minValue, maxValue);
        }
        public static double NextDouble()
        {
            return shared.NextDouble();
        }
    }
}
