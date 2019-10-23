using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    public class EvenGenerator : IntGenerator
    {
        public new object CreateInstance()
        {
            int num = (int)base.CreateInstance();
            num = num + (num % 2);
            return num;
        }
    }
}
