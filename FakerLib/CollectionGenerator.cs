using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerLib
{
    public interface ICollectionGenerator
    {
        object Generate(Type type);
    }
    public class ListGenerator : ICollectionGenerator
    {
        public object Generate(Type type)
        {
            IList instance = /*new List<typeof(type)>();*/(IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            int minSize = 4;
            int maxSize = 11;
            Random random = new Random();
            int listSize = random.Next(minSize, maxSize);
            for (int i = 0; i < listSize; i++)
            {
                ((IList)instance).Add(Generators.Create(type));
            }
            return instance;
        }
    }
}
