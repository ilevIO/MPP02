using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedSymbolGenerator
{
    public class ExtendedSymbolGenerator : Common.CharGenerator
    {
        public override object CreateInstance()
        {
            return Convert.ToChar(Common.ConvenienceRandom.Next(0, 127));
        }

        public override Type GeneratedType()
        {
            return typeof(char);
        }
    }
}
