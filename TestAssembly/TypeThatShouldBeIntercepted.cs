﻿using System;
using CryoAOP.Core;
using CryoAOP.Core.Extensions;

namespace CryoAOP.TestAssembly
{
    public class TypeThatShouldBeIntercepted
    {
        public void HavingMethodWithNoArgsAndNoReturnType()
        {
        }

        public int HavingMethodWithNoArgsAndInt32ReturnType()
        {
            return 1;
        }

        public void HavingMethodWithArgsAndNoReturnType(int arg1, string arg2, double arg3)
        {
            var type = typeof(TypeThatShouldBeIntercepted);
            var method = type.GetMethod("HavingMethodWithNoArgsAndNoReturnType");
            var invocation = new MethodInvocation(type, method, arg1, arg2, arg3);
            GlobalInterceptor.HandleInvocation(invocation);
        }

        public string HavingMethodWithArgsAndStringReturnType(int arg1, string arg2, double arg3)
        {
            return "{0}, {1}, {2}".FormatWith(arg1, arg2, arg3);
        }

        public void HavingMethodWithClassArgsAndNoReturnType(MethodParameterClass arg1)
        {
        }

        public MethodParameterClass HavingMethodWithClassArgsAndClassReturnType(MethodParameterClass arg1)
        {
            return new MethodParameterClass();
        }
    }

    public class MethodParameterClass
    {
        public int Arg1 = -1;
    }
}