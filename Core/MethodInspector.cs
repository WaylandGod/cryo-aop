﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryoAOP.Core.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CryoAOP.Core
{
    public class MethodInspector
    {
        public readonly TypeInspector TypeInspector;
        public readonly MethodDefinition Definition;

        public MethodInspector(TypeInspector typeInspector, MethodDefinition definition)
        {
            TypeInspector = typeInspector;
            Definition = definition;
        }

        public void Write(string assemblyPath)
        {
            TypeInspector.AssemblyInspector.Write(assemblyPath);
        }

        public void InterceptMethod(string methodPrefix)
        {
            // Create new Method 
            var interceptorMethod = new MethodDefinition(Definition.Name, Definition.Attributes, Definition.ReturnType);
            Definition.DeclaringType.Methods.Add(interceptorMethod);
            
            // Rename existing method 
            var renamedMethod = Definition;
            renamedMethod.Name = "{0}{1}".FormatWith(methodPrefix, renamedMethod.Name);

            // Copy parameters across
            if (Definition.HasParameters)
                foreach (var parameter in Definition.Parameters)
                 interceptorMethod.Parameters.Add(parameter);   

            // Insert interceptor code
            
            // Interceptor: Insert variables 
            var v_0 = new VariableDefinition("V_0", TypeInspector.AssemblyInspector.Import(typeof(Type)));
            interceptorMethod.Body.Variables.Add(v_0);
            var v_1 = new VariableDefinition("V_1", TypeInspector.AssemblyInspector.Import(typeof(MethodInfo)));
            interceptorMethod.Body.Variables.Add(v_1);
            var v_2 = new VariableDefinition("V_2", TypeInspector.AssemblyInspector.Import(typeof(MethodInvocation)));
            interceptorMethod.Body.Variables.Add(v_2);
            var v_3 = new VariableDefinition("V_3", TypeInspector.AssemblyInspector.Import(typeof(Object[])));
            interceptorMethod.Body.Variables.Add(v_3);
            interceptorMethod.Body.InitLocals = true;

            // Interceptor: Insert interception IL
            interceptorMethod.Nop();
            
            // Interceptor: Resolve type from handle uses V_0

            interceptorMethod.Ldtoken(TypeInspector.Definition);
            var getTypeFromHandleMethodReference = TypeInspector.AssemblyInspector.Import(typeof(Type), "GetTypeFromHandle");
            interceptorMethod.Call(getTypeFromHandleMethodReference);
            interceptorMethod.Stloc_0();

            // Interceptor: Get the method info 
            interceptorMethod.Ldloc_0();
            interceptorMethod.Ldstr(interceptorMethod.Name);
            var getMethodMethodReference = TypeInspector.AssemblyInspector.Import(typeof(Type), "GetMethod,String");
            interceptorMethod.Callvirt(getMethodMethodReference);
            interceptorMethod.Stloc_1();

            // Interceptor: Initialise object array for param values 
            interceptorMethod.Ldc_I4_S((sbyte)interceptorMethod.Parameters.Count);
            var objectReference = TypeInspector.AssemblyInspector.Import(typeof(Object));
            interceptorMethod.Newarr(objectReference);
            interceptorMethod.Stloc_3();

            foreach (var parameter in interceptorMethod.Parameters)
            {
                interceptorMethod.Ldloc_3();
                var argIndex = interceptorMethod.Parameters.IndexOf(parameter);
                interceptorMethod.Ldc_I4_S((sbyte)argIndex);
                interceptorMethod.Ldarg((ushort)(argIndex + 1));
                if (parameter.ParameterType.IsValueType)
                    interceptorMethod.Box(parameter.ParameterType);
                interceptorMethod.Stelem_ref();
            }

            // Inteceptor: Initialise Method Invocation
            var methodInvocationTypRef = TypeInspector.AssemblyInspector.Import(typeof(MethodInvocation));
            var methodInvocationConstructor = methodInvocationTypRef.Resolve().Methods.Where(m => m.IsConstructor).First();
            var methodInvocationConstructorReference = TypeInspector.AssemblyInspector.Import(methodInvocationConstructor);
            interceptorMethod.Ldloc_0();
            interceptorMethod.Ldloc_1();
            interceptorMethod.Ldloc_3();
            interceptorMethod.NewObj(methodInvocationConstructorReference);
            interceptorMethod.Stloc_2();

            // Interceptor: Call interceptor method 
            var methodInterceptorRef = TypeInspector.AssemblyInspector.Import(typeof(GlobalInterceptor), "HandleInvocation");
            interceptorMethod.Ldloc_2();
            interceptorMethod.Call(methodInterceptorRef);

            // Insert IL call from clone to renamed method
            interceptorMethod.Ldarg_0();
            foreach (var parameter in interceptorMethod.Parameters)
            {
                var argIndex = interceptorMethod.Parameters.IndexOf(parameter);
                interceptorMethod.Ldarg((ushort)(argIndex+1));
            }

            interceptorMethod.Call(renamedMethod);
            interceptorMethod.Nop();
            interceptorMethod.Ret();

        }

        public override string ToString()
        {
            return "{0}".FormatWith(Definition.FullName);
        }
    }
}