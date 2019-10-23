using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakerLib
{
    internal class Faker
    {
        private List<FakerConfig> configs = new List<FakerConfig>();
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
        public void AddConfig(FakerConfig config)
        {
            this.configs.Add(config);
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
                            for (int j = 0; j < this.configs.Count; j++)
                            {
                                var config = this.configs[j];
                                Type configType = config.GetType();
                                var genericParameters = configType.GetGenericArguments();
                            }
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
                        for (int j = 0; j < this.configs.Count; j++)
                        {
                            var config = this.configs[j];
                            Type configType = config.GetType();
                            var genericParameters = configType.GetGenericArguments();
                            
                        }
                        var fieldValue = Create(field.FieldType);
                        field.SetValue(generated, fieldValue);
                    }
                    foreach (var property in properties)
                    {
                        var propertyValue = Create(property.PropertyType);
                        if (property.CanWrite)
                        {
                            property.SetValue(generated, propertyValue);
                        }
                    }
                    this.generationStack.Pop();
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
