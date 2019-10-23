using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    public class IntGenerator : Common.IGenerator
    {
        public object CreateInstance()
        {
            return (Int32)(Common.ConvenienceRandom.Next(1, Int32.MaxValue));
        }

        public Type GeneratedType()
        {
            return typeof(Int32);
        }
    }
}
