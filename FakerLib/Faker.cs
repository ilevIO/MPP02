using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakerLib
{

    public class Generators
    {
        static Dictionary<Type, Common.IGenerator> availiableGenerators = new Dictionary<Type, Common.IGenerator>();
        //static Dictionary<Type>
        public static ListGenerator listGenerator = new ListGenerator();
        public static Common.IGenerator GetGeneratorByType(Type type)
        {
            foreach (KeyValuePair<Type, Common.IGenerator> generator in availiableGenerators)
            {
                if (generator.Value.GeneratedType() == type) {
                    return generator.Value;
                }
            }
            return null;
        }
        //public static Generators shared = new Generators();
        public void AddGenerator(Common.IGenerator newGenerator)
        {
            availiableGenerators.Add(newGenerator.GetType(), newGenerator);

        }
        static Generators()
        {
            availiableGenerators.Add(typeof(int), new IntGenerator());
            availiableGenerators.Add(typeof(char), new LatinLetterGenerator());
            availiableGenerators.Add(typeof(string), new StringGenerator());
            String dllPath = "C:\\Users\\ilyayelagov\\source\\repos\\Faker\\FloatGenerator\\bin\\Debug\\FloatGenerator.dll";
            if (File.Exists(dllPath))
            {
                Assembly asm = Assembly.LoadFrom(dllPath);
                var types = asm.GetTypes().Where(t => t.GetInterfaces().Where(i => i.Equals(typeof(Common.IGenerator))).Any());

                foreach (var type in types)
                {
                    var plugin = asm.CreateInstance(type.FullName) as Common.IGenerator;
                    Type t = plugin.GeneratedType();
                    if (!availiableGenerators.ContainsKey(t))
                        availiableGenerators.Add(t, plugin);
                }
            }
            /*this.AddGenerator(new IntGenerator());
            this.AddGenerator(new LatinLetterGenerator());
            this.AddGenerator(new StringGenerator());*/
            //shared.AddGenerator(new IntGenerator());
        }
        private static Faker faker = new Faker();
        public static object Create(Type objectType)
        {
            return faker.Create(objectType);
        }
    }

    internal class Faker
    {
        //private Generators generators = new Generators();
        private Stack<Type> generationStack = new Stack<Type>();
        private bool isBeingGenerated(Type type)
        {
            foreach (Type generatedType in generationStack)
            {
                if (type == generatedType)
                {
                    return true;
                }
            }
            return false;
        }
        public object Create(Type type)
        {
            var objectType = type;
            object generated = null;
            if (isBeingGenerated(objectType))
            {
                return null;
            }
            var generator = /*Generators.shared.*/Generators.GetGeneratorByType(objectType);
            if (generator != null)
            {
                generated = /*Generators.shared.*/generator.CreateInstance();
            }
            else
            {
                if (!objectType.IsPrimitive/*objectType.IsClass*/ && !objectType.IsAbstract && !objectType.IsGenericType)
                {
                    this.generationStack.Push(objectType);
                    //generated = ClassGenerator.generate(T) {
                    var constructorsList = objectType.GetConstructors();
                    if (constructorsList.Length > 0)
                    {
                        ConstructorInfo suitableConstructorInfo = constructorsList[0];
                        int maxParamsNum = suitableConstructorInfo.GetParameters().Length;

                        for (int i = 1; i < constructorsList.Length; i++)
                        {
                            var paramsLen = constructorsList[i].GetParameters().Length;
                            if (paramsLen > maxParamsNum)
                            {
                                maxParamsNum = paramsLen;
                                suitableConstructorInfo = constructorsList[i];
                            }
                        }
                        var parameters = suitableConstructorInfo.GetParameters();

                        List<object> paramsValues = new List<object>();
                        for (int i = 0; i < maxParamsNum; i++)
                        {
                            Type parameterType = parameters[i].ParameterType;
                            object paramValue = this.Create(parameterType);
                            paramsValues.Add(paramValue);
                        }
                        generated = suitableConstructorInfo.Invoke(paramsValues.ToArray());
                    }
                    else
                    {
                        generated = Activator.CreateInstance(objectType);
                    }
                    var properties = objectType.GetProperties();
                    var otherFields = objectType.GetFields();
                    foreach (var field in otherFields)
                    {
                        var fieldValue = Create(field.FieldType);
                        field.SetValue(generated, fieldValue);
                    }
                    foreach (var property in type.GetProperties())
                    {
                        var propertyValue = Create(property.PropertyType);
                        if (property.CanWrite)
                        {
                            property.SetValue(generated, propertyValue);
                        }
                    }
                    this.generationStack.Pop();
                    //}
                }
                else if (objectType.IsGenericType)
                {
                    //handle collections;
                    generated = Generators.listGenerator.Generate(objectType.GetGenericArguments()[0]);
                }
                else if (!objectType.IsPrimitive && objectType.IsValueType)
                {
                    //handle structs;
                    generated = Activator.CreateInstance(objectType);
                    var otherFields = objectType.GetFields();
                    foreach (var field in otherFields)
                    {
                        var fieldValue = Create(field.FieldType);
                        field.SetValue(generated, fieldValue);
                    }
                }
                else if (objectType.IsGenericType)
                {
                    ICollectionGenerator collectionGenerator = Generators.GetGeneratorByType(objectType) as ICollectionGenerator;
                    if (collectionGenerator != null)
                    {
                        generated = collectionGenerator.Generate(objectType.GenericTypeArguments[0]);
                    }
                }
            }
            return generated;
        }
        public T Create<T>()
        {
            Type objectType = typeof(T);
            return (T)Create(objectType);
            
        }
    }
}
