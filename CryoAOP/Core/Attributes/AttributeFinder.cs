using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryoAOP.Exec;

namespace CryoAOP.Core.Attributes
{
    internal class AttributeFinder
    {
        private readonly AssemblyLoader loader = new AssemblyLoader();

        public IEnumerable<AttributeResult<T>> FindAttributes<T>() where T : Attribute
        {
            var assemblies = loader.GetShadowAssemblies();
            var attributesFound = new List<AttributeResult<T>>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.ShadowAssembly.GetTypes())
                    {
                        FindMethodAttributes(assembly, type, attributesFound);
                        FindTypeAttributes(assembly, type, attributesFound);
                    }
                }
                catch (Exception err3)
                {
                    "CryoAOP -> Warning! First chance exception ocurred while searching for Mixin Methods. \r\n{0}"
                        .Warn(err3);
                }
            }
            return attributesFound;
        }

        private static void FindMethodAttributes<T>(ShadowAssemblyType shadowAssembly, Type type, List<AttributeResult<T>> attributesFound) where T : Attribute
        {
            try
            {
                var methods =
                    type.GetMethods(
                        BindingFlags.Public
                        | BindingFlags.NonPublic
                        | BindingFlags.Static
                        | BindingFlags.Instance);

                foreach (var method in methods)
                {
                    try
                    {
                        var methodAttributes =
                            method
                                .GetCustomAttributes(true)
                                .Where(
                                    attr =>
                                    attr.GetType().FullName == typeof (T).FullName)
                                .ToList();

                        if (methodAttributes.Count > 0)
                        {
                            var attribute = methodAttributes.Cast<T>().First();
                            var info = new AttributeResult<T>(shadowAssembly, type, method, attribute);
                            attributesFound.Add(info);
                        }
                    }
                    catch (Exception err)
                    {
                        "CryoAOP -> Warning! First chance exception ocurred while searching for Mixin Methods. \r\n{0}"
                            .Warn(err);
                    }
                }
            }
            catch (Exception err2)
            {
                "CryoAOP -> Warning! First chance exception ocurred while searching for Mixin Methods. \r\n{0}"
                    .Warn(err2);
            }
        }

        private static void FindTypeAttributes<T>(ShadowAssemblyType shadowAssembly, Type type, List<AttributeResult<T>> attributesFound) where T : Attribute
        {
            try
            {
                var typeAttributes =
                    type
                        .GetCustomAttributes(true)
                        .Where(
                            attr =>
                            attr.GetType().FullName == typeof (T).FullName)
                        .ToList();

                if (typeAttributes.Count > 0)
                {
                    var attribute = typeAttributes.Cast<T>().First();
                    var info = new AttributeResult<T>(shadowAssembly, type, attribute);
                    attributesFound.Add(info);
                }
            }
            catch (Exception err1)
            {
                "CryoAOP -> Warning! First chance exception ocurred while searching for Mixin Methods. \r\n{0}"
                    .Warn(err1);
            }
        }
    }
}