using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakerTests
{
    [TestClass]
    public class UnitTest1
    {
        class ClassOfPrimitives
        {
            public int integer;
            public char someChar;
            public string someString;
        }
        class ClassOfPrimitivesWithConstructors
        {
            private int propertyValue;
            private int constructedInteger;
            private int secondConstructedInteger;
            public int getConstructedInteger()
            {
                return constructedInteger;
            }
            public int getSecondConstructedInteger()
            {
                return secondConstructedInteger;
            }
            public int propertyInt
            {
                get
                {
                    return propertyValue;
                }
                set
                {
                    propertyValue = value;
                }
            }
            public ClassOfPrimitivesWithConstructors(int value)
            {
                this.constructedInteger = value;
            }
            public ClassOfPrimitivesWithConstructors(int value1, int value2)
            {
                this.constructedInteger = value1;
                this.secondConstructedInteger = value2;
            }
        }
        class RecursiveClass
        {
            public RecursiveClass innerInstance;
        }
        class LowerInterrecursiveClass
        {
            public UpperInterrecursiveClass innerInstance;
        }
        class IntermediateInterrecursiveClass
        {
            public LowerInterrecursiveClass innerInstance;
        }
        class UpperInterrecursiveClass
        {
            public IntermediateInterrecursiveClass innerInstance;
        }

        class ParentClass
        {
            protected int constructedValue;
            public int value;
            public int getConstructedValue
            {
                get
                {
                    return constructedValue;
                }
            }
            public ParentClass (int value)
            {
                this.constructedValue = value;
            }
        }
        class ChildClass: ParentClass
        {
            private int childConstructedValue;
            public int getChildConstructedValue
            {
                get
                {
                    return childConstructedValue;
                }
            }
            public ChildClass (int value) : base(value)
            {
                constructedValue = value;
                childConstructedValue = value / 2;
            }
        }
        class AbscentType
        {
            public double value;
        }
        struct StructType
        {
            public int integer;
        }
        class ListClass
        {
            public List<StructType> list;
        }
        [TestMethod]
        public void ClassOfPrimitivesTest()
        {
            ClassOfPrimitives instance = (ClassOfPrimitives)FakerLib.Generators.Create(typeof(ClassOfPrimitives));
            Assert.AreNotEqual(default(int), instance.integer);
            Assert.AreNotEqual(default(char), instance.someChar);
            Assert.AreNotEqual(default(string), instance.someString);
        }
        [TestMethod]
        public void ClassOfPrimitivesWithConstructorTest()
        {
            ClassOfPrimitivesWithConstructors instance = (ClassOfPrimitivesWithConstructors)FakerLib.Generators.Create(typeof(ClassOfPrimitivesWithConstructors));
            Assert.AreNotEqual(default(int), instance.propertyInt);
            Assert.AreNotEqual(default(int), instance.getConstructedInteger());
            Assert.AreNotEqual(default(int), instance.getSecondConstructedInteger());
        }
        [TestMethod] 
        public void InterreqursionTest()
        {
            UpperInterrecursiveClass instance = (UpperInterrecursiveClass)FakerLib.Generators.Create(typeof(UpperInterrecursiveClass));
            Assert.IsNotNull(instance.innerInstance);
            Assert.IsNotNull(instance.innerInstance.innerInstance);
            Assert.IsNull(instance.innerInstance.innerInstance.innerInstance);
        }

        [TestMethod]
        public void InheritanceTest()
        {

            ChildClass instance = (ChildClass)FakerLib.Generators.Create(typeof(ChildClass));
            Assert.AreNotEqual(default(int), instance.getChildConstructedValue);
            Assert.AreNotEqual(default(int), instance.getConstructedValue);
        }
        [TestMethod] 
        public void AbscentTypeTest ()
        {
            AbscentType instance = (AbscentType)FakerLib.Generators.Create(typeof(AbscentType));
            Assert.AreEqual(default(double), instance.value);
        }
        [TestMethod]
        public void StructTest()
        {
            StructType instance = (StructType)FakerLib.Generators.Create(typeof(StructType));
            Assert.AreNotEqual(default(int), instance.integer);
        }
        [TestMethod] 
        public void ListOfStructTest()
        {
            ListClass instance = (ListClass)FakerLib.Generators.Create(typeof(ListClass));
            Assert.IsTrue(instance.list.Count >= 4 && instance.list.Count <= 11);
            Assert.AreNotEqual(default(int), instance.list[0]);
        }
        [TestMethod] 
        public void FloatGeneratorTest()
        {
            float instance = (float)FakerLib.Generators.Create(typeof(float));
            float eps = float.Epsilon;
            Assert.IsTrue(instance > default(float) + eps);
        }
    }
}
