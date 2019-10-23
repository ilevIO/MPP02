using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatGenerator
{
    public class FloatGenerator : Common.IGenerator
    {
        public object CreateInstance()
        {
            return (float)(Common.ConvenienceRandom.NextDouble());
        }

        public Type GeneratedType()
        {
            return typeof(float);
        }
    }
}
