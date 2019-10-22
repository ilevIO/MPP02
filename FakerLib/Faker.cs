using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace FakerLib
{

    class Generators
    {
        Dictionary<Type, IGenerator> availiableGenerators = new Dictionary<Type, IGenerator>();
        public IGenerator GetGeneratorByType(Type type)
        {
            foreach (KeyValuePair<Type, IGenerator> generator in availiableGenerators)
            {
                if (generator.Value.GeneratedType() == type) {
                    return generator.Value;
                }
            }
            return null;
        }
        //public static Generators shared = new Generators();
        public void AddGenerator(IGenerator newGenerator)
        {
            availiableGenerators.Add(newGenerator.GetType(), newGenerator);

        }
        /*private*/ public Generators()
        {
            this.AddGenerator(new IntGenerator());
            this.AddGenerator(new LatinLetterGenerator());
            this.AddGenerator(new StringGenerator());
            //shared.AddGenerator(new IntGenerator());
        }
    }
    public interface IGenerator
    {
        //bool GeneratesType(Type type);
        Type GeneratedType();
        object CreateInstance();
        //T CreateInstance<T>();
    }
    public class Faker
    {
        private Generators generators = new Generators();
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
            var generator = /*Generators.shared.*/generators.GetGeneratorByType(objectType);
            if (generator != null)
            {
                generated = /*Generators.shared.*/generator.CreateInstance();
            }
            else
            {
                if (objectType.IsClass && !objectType.IsAbstract)
                {
                    this.generationStack.Push(objectType);
                    //generated = ClassGenerator.generate(T) {
                    var constructorsList = objectType.GetConstructors();
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
                    var properties = objectType.GetProperties();
                    var parameters = suitableConstructorInfo.GetParameters();

                    List<object> paramsValues = new List<object>();
                    for (int i = 0; i < maxParamsNum; i++)
                    {
                        Type parameterType = parameters[i].ParameterType;
                        object paramValue = this.Create(parameterType);
                        paramsValues.Add(paramValue);
                    }
                    generated = suitableConstructorInfo.Invoke(paramsValues.ToArray());

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
                else if (objectType.IsValueType)
                {
                    //handle structs;
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
