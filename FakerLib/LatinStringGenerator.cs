using System;
using System.Collections.Generic;
using System.Text;

namespace FakerLib
{
    class StringGenerator : IGenerator
    {
        private int minLength = 4;
        private int maxLength = 11;
        static CharGenerator charGenerator = new LatinLetterGenerator();
        public object CreateInstance()
        {
            char[] symbols = new char[ConvenienceRandom.Next(minLength, maxLength)];
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
    }
}
