﻿namespace CryoAOP.Core.Extensions
{
    internal static class CryoAOPExtensions
    {
        public static MethodIntercept GetMethod(this string[] args)
        {
            var assemblyInspector = new AssemblyIntercept(args[0].Trim());
            var typeInspector = assemblyInspector.FindType(args[1].Trim());
            var methodInspector = typeInspector.FindMethod(args[2].Trim());
            return methodInspector;
        }
    }
}