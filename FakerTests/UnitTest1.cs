using System;
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
        [TestMethod]
        public void ClassOfPrimitivesTest()
        {
            FakerLib.Faker faker = new FakerLib.Faker();
            ClassOfPrimitives instance = (ClassOfPrimitives)faker.Create(typeof(ClassOfPrimitives));
            Assert.IsNotNull(instance.integer);
            Assert.IsNotNull(instance.someChar);
            Assert.IsNotNull(instance.someString);
        }
        [TestMethod]
        public void ClassOfPrimitivesWithConstructorTest()
        {
            FakerLib.Faker faker = new FakerLib.Faker();
            ClassOfPrimitivesWithConstructors instance = (ClassOfPrimitivesWithConstructors)faker.Create(typeof(ClassOfPrimitivesWithConstructors));
            Assert.IsNotNull(instance.propertyInt);
            Assert.IsNotNull(instance.getConstructedInteger());
            Assert.IsNotNull(instance.getSecondConstructedInteger());
        }
        [TestMethod] 
        public void InterreqursionTest()
        {
            FakerLib.Faker faker = new FakerLib.Faker();
            UpperInterrecursiveClass instance = (UpperInterrecursiveClass)faker.Create(typeof(UpperInterrecursiveClass));
            Assert.IsNotNull(instance.innerInstance);
            Assert.IsNotNull(instance.innerInstance.innerInstance);
            Assert.IsNull(instance.innerInstance.innerInstance.innerInstance);
        }

        [TestMethod]
        public void InheritanceTest()
        {
            FakerLib.Faker faker = new FakerLib.Faker();
            ChildClass instance = (ChildClass)faker.Create(typeof(ChildClass));
            Assert.IsNotNull(instance.getChildConstructedValue);
            Assert.IsNotNull(instance.getConstructedValue);
        }
    }
}
