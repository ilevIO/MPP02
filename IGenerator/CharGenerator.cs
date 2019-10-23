using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class CharGenerator : IGenerator
    {
        public abstract object CreateInstance();
        public abstract Type GeneratedType();
    }
}
