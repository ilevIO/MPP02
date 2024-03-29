﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    
    
    class LatinLetterGenerator : Common.CharGenerator
    {
        public override object CreateInstance()
        {
            return Convert.ToChar(Common.ConvenienceRandom.Next(65, 122));
        }

        public override Type GeneratedType()
        {
            return typeof(char);
        }
    }
}
