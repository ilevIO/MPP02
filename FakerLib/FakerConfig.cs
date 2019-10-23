﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FakerLib
{
    public class FakerConfig
    {
        public void Add <ClassType, FieldType, Generator> (Expression<Func<ClassType, FieldType>> expression)
        {
            Common.IGenerator generator = (Common.IGenerator)Activator.CreateInstance(typeof(Generator));
          
        }
    }
}
