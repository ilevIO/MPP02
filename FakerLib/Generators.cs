using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FakerLib
{
    public class Generators
    {
        static Dictionary<Type, Common.IGenerator> availiableGenerators = new Dictionary<Type, Common.IGenerator>();

        public static ListGenerator listGenerator = new ListGenerator();
        public static Common.IGenerator GetGeneratorByType(Type type)
        {
            foreach (KeyValuePair<Type, Common.IGenerator> generator in availiableGenerators)
            {
                if (generator.Value.GeneratedType() == type)
                {
                    return generator.Value;
                }
            }
            return null;
        }
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
        }
        private static Faker faker = new Faker();
        public static object Create(Type objectType)
        {
            return faker.Create(objectType);
        }
        public static T Create<T>()
        {
            return (T)Generators.Create(typeof(T));
        } 
        public static void AddConfig(FakerConfig config)
        {
            faker.AddConfig(config);
        }
    }
}
