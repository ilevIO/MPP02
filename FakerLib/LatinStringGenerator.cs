using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    class StringGenerator : Common.IGenerator
    {
        private int minLength = 4;
        private int maxLength = 11;
        Common.CharGenerator charGenerator = new LatinLetterGenerator();
        public object CreateInstance()
        {
            char[] symbols = new char[Common.ConvenienceRandom.Next(minLength, maxLength)];
            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = (char)charGenerator.CreateInstance();
            }
            return new string(symbols);
        }
        public Type GeneratedType()
        {
            return typeof(String);
        }
        public StringGenerator()
        {
        }
        public StringGenerator(Common.CharGenerator charGenerator)
        {
            this.charGenerator = charGenerator;
        }
    }
}
