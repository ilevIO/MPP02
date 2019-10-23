using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    class IntGenerator : Common.IGenerator
    {
        /*static*/ public object CreateInstance()
        {
            return (Int32)(Common.ConvenienceRandom.Next(1, Int32.MaxValue));
        }

        /*static*/ public Type GeneratedType()
        {
            return typeof(Int32);
        }
    }
}
