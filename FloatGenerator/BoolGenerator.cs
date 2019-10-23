using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloatGenerator
{
    public class BoolGenerator : Common.IGenerator
    {
        public object CreateInstance()
        {
            return true;
        }

        public Type GeneratedType()
        {
            return typeof(bool);
        }
    }
}
