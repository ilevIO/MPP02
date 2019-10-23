using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IGenerator
    {
        //bool GeneratesType(Type type);
        Type GeneratedType();
        object CreateInstance();
        //T CreateInstance<T>();
    }
}
