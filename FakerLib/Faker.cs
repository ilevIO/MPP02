using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace FakerLib
{

    class Generators
    {
        Dictionary<Type, IGenerator> availiableGenerators;
        public IGenerator GetGeneratorByType(Type type)
        {
            foreach (KeyValuePair<Type, IGenerator> generator in availiableGenerators)
            {
                if (generator.Value.GeneratesType(type)) {
                    return generator.Value;
                }
            }
            return null;
        }
        public void AddGenerator(IGenerator newGenerator)
        {
            availiableGenerators.Add(newGenerator.GetType(), newGenerator);
        }
    }
    public interface IGenerator
    {
        bool GeneratesType(Type type);
        Type GeneratedType();
        object CreateInstance();
        T CreateInstance<T>();
    }
    public class Faker
    {
        private Type parentType = null;
        Generators generators;
  
        private Stack<Type> generationStack;
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
                    property.SetValue(generated, propertyValue);
                }
                this.generationStack.Pop();
                //}
            }
            else if (objectType.IsGenericType)
            {
                generated = generators.GetGeneratorByType(objectType).CreateInstance();
            }
            return generated;
        }
        public T Create<T>()
        {

            object generated = null;
            Type objectType = typeof(T);
            return (T)Create(objectType);
            
        }
    }
}
